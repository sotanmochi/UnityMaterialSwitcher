using UnityEngine;
using RenderQueue = UnityEngine.Rendering.RenderQueue;

namespace MaterialSwitcher
{
    /// <summary>
    /// Converter from lilToon to MToon10 material.
    /// </summary>
    public class LilToonToMToonConverter : IMaterialConverter
    {
        public string SourceMaterialTypeId => "lilToon";
        public string TargetMaterialTypeId => "MToon10";

        public bool CanConvert(Material sourceMaterial, Material targetMaterial)
        {
            if (sourceMaterial == null || targetMaterial == null)
            {
                return false;
            }

            return sourceMaterial.shader?.name.Contains("lilToon") == true &&
                   targetMaterial.shader?.name.Contains("MToon10") == true;
        }

        /// <summary>
        /// Set up MToon10 material properties from lilToon material. <br/>
        /// 
        /// References: <br/>
        ///  - https://github.com/lilxyzw/lilToon/blob/1.8.5/Assets/lilToon/Editor/lilInspector.cs#L4181 <br/>
        ///  - https://github.com/vrm-c/UniVRM/blob/v0.129.3/Assets/VRM10/MToon10/Runtime/UnityShaderLab/Properties/MToon10Properties.cs <br/>
        ///  - https://github.com/vrm-c/UniVRM/blob/v0.129.3/Assets/VRM10/MToon10/Shaders/vrmc_materials_mtoon_urp.shader <br/>
        ///  - https://github.com/vrm-c/UniVRM/blob/v0.129.3/Assets/VRM10/MToon10/Shaders/vrmc_materials_mtoon.shader <br/>
        /// 
        /// </summary>
        /// <param name="liltoon"></param>
        /// <param name="mtoon"></param>
        public void Convert(Material liltoon, Material mtoon)
        {
            if (!CanConvert(liltoon, mtoon))
            {
                UnityLogger.LogError($"[{nameof(LilToonToMToonConverter)}] Cannot convert: incompatible materials.");
                return;
            }

            UnityLogger.Log($"<color=cyan>[{nameof(LilToonToMToonConverter)}] Convert lilToon to MToon10 ({liltoon.name} -> {mtoon.name})</color>");

            // Cull Mode (Double Sided)
            var doubleSided = liltoon.GetInt(lilToonPropertyType.Cull) == (int)lilToonCullMode.Off
                            ? MToon10DoubleSidedMode.On
                            : MToon10DoubleSidedMode.Off;
            mtoon.SetInt(MToon10PropertyType.DoubleSided, (int)doubleSided);

            // Main Color
            var mainColor = liltoon.GetColor(lilToonPropertyType.MainColor);
            var clampedMainColor = new Color(
                Mathf.Clamp01(mainColor.r),
                Mathf.Clamp01(mainColor.g),
                Mathf.Clamp01(mainColor.b),
                Mathf.Clamp01(mainColor.a)
            );
            mtoon.SetColor(MToon10PropertyType.BaseColorFactor, clampedMainColor);

            // Main Texture
            var mainTex = liltoon.GetTexture(lilToonPropertyType.MainTex);
            var mainTexScale = liltoon.GetTextureScale(mainTex.name);
            var mainTexOffset = liltoon.GetTextureOffset(mainTex.name);
            mtoon.SetTexture(MToon10PropertyType.BaseColorTexture, mainTex);
            mtoon.SetTextureScale(mainTex.name, mainTexScale);
            mtoon.SetTextureOffset(mainTex.name, mainTexOffset);

            // Normal Map
            var useNormalMap = liltoon.GetFloat(lilToonPropertyType.UseNormalMap) == 1.0f;
            var normalTex = liltoon.GetTexture(lilToonPropertyType.NormalTex);
            if (useNormalMap && normalTex != null)
            {
                mtoon.SetTexture(MToon10PropertyType.NormalTexture, normalTex);
                mtoon.SetFloat(MToon10PropertyType.NormalTextureScale, liltoon.GetFloat(lilToonPropertyType.NormalTexScale));
                mtoon.EnableKeyword("_NORMALMAP");
            }

            // Shadow
            var useShadow = liltoon.GetFloat(lilToonPropertyType.UseShadow) == 1.0f;
            if (useShadow)
            {
                // Calculate ShadingShiftFactor from ShadowBorder.
                var shadowBorder = liltoon.GetFloat(lilToonPropertyType.ShadowBorder); // [0.0, 1.0]
                var shadeShift = -2.0f * shadowBorder + 1.0f; // [-1.0, 1.0]

                // Calculate ShadingToonyFactor from ShadowBlur.
                var shadowBlur = liltoon.GetFloat(lilToonPropertyType.ShadowBlur); // [0.0, 1.0]
                var shadeToony = 1.0f - shadowBlur; // [0.0, 1.0]

                mtoon.SetFloat(MToon10PropertyType.ShadingShiftFactor, shadeShift);
                mtoon.SetFloat(MToon10PropertyType.ShadingToonyFactor, shadeToony);

                var shadowMainStrength = liltoon.GetFloat(lilToonPropertyType.ShadowMainStrength);
                var shadowStrengthMask = liltoon.GetTexture(lilToonPropertyType.ShadowStrengthMask);
                if (shadowStrengthMask != null && shadowMainStrength != 0.0f)
                {
                    mtoon.SetColor(MToon10PropertyType.ShadeColorFactor, Color.white);
                    mtoon.SetTexture(MToon10PropertyType.ShadeColorTexture, mainTex);
                }
                else
                {
                    var shadowColor = liltoon.GetColor(lilToonPropertyType.ShadowColor);
                    var shadowStrength = liltoon.GetFloat(lilToonPropertyType.ShadowStrength);
                    var shadeColorStrength = new Color(
                        1.0f - ((1.0f - shadowColor.r) * shadowStrength),
                        1.0f - ((1.0f - shadowColor.g) * shadowStrength),
                        1.0f - ((1.0f - shadowColor.b) * shadowStrength),
                        1.0f
                    );
                    mtoon.SetColor(MToon10PropertyType.ShadeColorFactor, shadeColorStrength);

                    var shadeColorTex = liltoon.GetTexture(lilToonPropertyType.ShadowColorTex);
                    if (shadeColorTex != null)
                    {
                        mtoon.SetTexture(MToon10PropertyType.ShadeColorTexture, shadeColorTex);
                    }
                    else
                    {
                        mtoon.SetTexture(MToon10PropertyType.ShadeColorTexture, mainTex);
                    }
                }
            }
            else
            {
                mtoon.SetColor(MToon10PropertyType.ShadeColorFactor, Color.white);
                mtoon.SetTexture(MToon10PropertyType.ShadeColorTexture, mainTex);
            }

            // Emission
            var useEmission = liltoon.GetFloat(lilToonPropertyType.UseEmission) == 1.0f;
            var emissionTex = liltoon.GetTexture(lilToonPropertyType.EmissionTex);
            if (useEmission && emissionTex != null)
            {
                mtoon.SetColor(MToon10PropertyType.EmissiveFactor, liltoon.GetColor(lilToonPropertyType.EmissionColor));
                mtoon.SetTexture(MToon10PropertyType.EmissiveTexture, emissionTex);
                mtoon.EnableKeyword("_MTOON_EMISSIVEMAP");
            }

            // Rim Light
            var useRim = liltoon.GetFloat(lilToonPropertyType.UseRim) == 1.0f;
            if (useRim)
            {
                var rimColorTex = liltoon.GetTexture(lilToonPropertyType.RimColorTex);
                var rimEnableLighting = liltoon.GetFloat(lilToonPropertyType.RimEnableLighting);

                mtoon.SetColor(MToon10PropertyType.ParametricRimColorFactor, liltoon.GetColor(lilToonPropertyType.RimColor));
                mtoon.SetTexture(MToon10PropertyType.RimMultiplyTexture, rimColorTex);
                mtoon.SetFloat(MToon10PropertyType.RimLightingMixFactor, rimEnableLighting);

                // Set default values for RimFresnelPower and RimLift, because it is difficult to calculate from lilToon parameters.
                // lilToonのパラメーターから計算することが困難なため、RimFresnelPowerとRimLiftの値はデフォルト値をセットする。
                var rimFresnelPower = 5.0f;
                var rimLift = 0f;
                mtoon.SetFloat(MToon10PropertyType.ParametricRimFresnelPowerFactor, rimFresnelPower);
                mtoon.SetFloat(MToon10PropertyType.ParametricRimLiftFactor, rimLift);

                mtoon.EnableKeyword("_MTOON_RIMMAP");
            }
            else
            {
                mtoon.SetColor(MToon10PropertyType.ParametricRimColorFactor, Color.black);
            }

            // MatCap
            var useMatCap = liltoon.GetFloat(lilToonPropertyType.UseMatCap) == 1.0f;
            var matcapTex = liltoon.GetTexture(lilToonPropertyType.MatCapTex);
            if (useMatCap && matcapTex != null)
            {
                mtoon.SetColor(MToon10PropertyType.MatcapColorFactor, liltoon.GetColor(lilToonPropertyType.MatCapColor));
                mtoon.SetTexture(MToon10PropertyType.MatcapTexture, matcapTex);
            }

            // Outline
            var isOutline = liltoon.shader.name.Contains("Outline");
            if (isOutline)
            {
                var outlineWidthMask = liltoon.GetTexture(lilToonPropertyType.OutlineWidthMask);
                mtoon.SetTexture(MToon10PropertyType.OutlineWidthMultiplyTexture, outlineWidthMask);

                var outlineWidthScaleFactor = 0.01f; // The outline width of lilToon is approximately 100 times that of MToon10.
                var outlineWidth = liltoon.GetFloat(lilToonPropertyType.OutlineWidth);
                mtoon.SetFloat(MToon10PropertyType.OutlineWidthFactor, outlineWidth * outlineWidthScaleFactor);

                var outlineColor = liltoon.GetColor(lilToonPropertyType.OutlineColor);
                mtoon.SetColor(MToon10PropertyType.OutlineColorFactor, outlineColor);

                mtoon.SetFloat(MToon10PropertyType.OutlineLightingMixFactor, 1.0f);
                mtoon.SetInt(MToon10PropertyType.OutlineWidthMode, (int)MToon10OutlineMode.World);

                mtoon.EnableKeyword("_MTOON_OUTLINE_WORLD");
            }

            // Rendering Mode
            var isTransparent = liltoon.shader.name.Contains("Transparent");
            var zwrite = liltoon.GetFloat(lilToonPropertyType.ZWrite) != 0.0f;
            var isCutout = liltoon.shader.name.Contains("Cutout");

            if (isCutout)
            {
                // Cutout Mode
                mtoon.SetInt(MToon10PropertyType.AlphaMode, (int)MToon10AlphaMode.Cutout);
                mtoon.SetFloat(MToon10PropertyType.AlphaCutoff, liltoon.GetFloat(lilToonPropertyType.Cutoff));
                mtoon.SetFloat("_BlendMode", 1.0f);
                mtoon.SetOverrideTag("RenderType", "TransparentCutout");
                mtoon.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
                mtoon.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);
                mtoon.SetFloat("_ZWrite", 1.0f);
                mtoon.SetFloat("_AlphaToMask", 1.0f);
                mtoon.EnableKeyword("_ALPHATEST_ON");
                mtoon.renderQueue = (int)RenderQueue.AlphaTest;
            }
            else if (isTransparent && !zwrite)
            {
                // Transparent Mode (ZWrite Off)
                mtoon.SetInt(MToon10PropertyType.AlphaMode, (int)MToon10AlphaMode.Transparent);
                mtoon.SetFloat("_BlendMode", 2.0f);
                mtoon.SetOverrideTag("RenderType", "TransparentCutout");
                mtoon.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mtoon.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mtoon.SetFloat("_ZWrite", 0.0f);
                mtoon.SetFloat("_AlphaToMask", 0.0f);
                mtoon.EnableKeyword("_ALPHABLEND_ON");
                mtoon.renderQueue = (int)RenderQueue.Transparent;
            }
            else if (isTransparent && zwrite)
            {
                // Transparent Mode (ZWrite On)
                mtoon.SetInt(MToon10PropertyType.AlphaMode, (int)MToon10AlphaMode.Transparent);
                mtoon.SetFloat("_BlendMode", 3.0f);
                mtoon.SetOverrideTag("RenderType", "TransparentCutout");
                mtoon.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mtoon.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mtoon.SetFloat("_ZWrite", 1.0f);
                mtoon.SetFloat("_AlphaToMask", 0.0f);
                mtoon.EnableKeyword("_ALPHABLEND_ON");
                mtoon.renderQueue = (int)RenderQueue.Transparent;
            }
            else
            {
                // Opaque Mode
                mtoon.SetInt(MToon10PropertyType.AlphaMode, (int)MToon10AlphaMode.Opaque);
                mtoon.SetFloat("_BlendMode", 0.0f);
                mtoon.SetOverrideTag("RenderType", "Opaque");
                mtoon.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
                mtoon.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);
                mtoon.SetFloat("_ZWrite", 1.0f);
                mtoon.SetFloat("_AlphaToMask", 0.0f);
                mtoon.renderQueue = -1;
            }
        }
    }
}
