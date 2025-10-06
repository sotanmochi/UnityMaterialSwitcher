using UnityEngine;
using RenderQueue = UnityEngine.Rendering.RenderQueue;

namespace MaterialSwitcher
{
    /// <summary>
    /// Converter from MToon10 to lilToon material.
    /// </summary>
    public class MToonToLilToonConverter : IMaterialConverter
    {
        public string SourceMaterialTypeId => "MToon10";
        public string TargetMaterialTypeId => "lilToon";

        public bool CanConvert(Material sourceMaterial, Material targetMaterial)
        {
            if (sourceMaterial == null || targetMaterial == null)
            {
                return false;
            }

            return sourceMaterial.shader?.name.Contains("MToon10") == true &&
                   targetMaterial.shader?.name.Contains("lilToon") == true;
        }

        /// <summary>
        /// Set up lilToon material properties from MToon10 material. <br/>
        /// 
        /// References: <br/>
        ///  - https://github.com/lilxyzw/lilMaterialConverter/blob/1.0.4/Assets/Editor/lilMaterialConverter.cs#L400 <br/>
        /// 
        /// </summary>
        /// <param name="mtoon"></param>
        /// <param name="liltoon"></param>
        public void Convert(Material mtoon, Material liltoon)
        {
            if (!CanConvert(mtoon, liltoon))
            {
                UnityLogger.LogError($"[{nameof(MToonToLilToonConverter)}] Cannot convert: incompatible materials.");
                return;
            }

            UnityLogger.Log($"<color=cyan>[{nameof(MaterialSwitcher)}] Convert MToon10 to lilToon ({mtoon.name} -> {liltoon.name})</color>");

            var hasNormalMap = mtoon.GetTexture(MToon10PropertyType.NormalTexture) != null;

            var hasShadow = mtoon.GetColor(MToon10PropertyType.ShadeColorFactor) != Color.black ||
                            mtoon.GetTexture(MToon10PropertyType.ShadeColorTexture) != null;

            var hasEmission = mtoon.GetColor(MToon10PropertyType.EmissiveFactor) != Color.black ||
                              mtoon.GetTexture(MToon10PropertyType.EmissiveTexture) != null;

            var hasRimLight = mtoon.GetColor(MToon10PropertyType.ParametricRimColorFactor) != Color.black ||
                              mtoon.GetTexture(MToon10PropertyType.RimMultiplyTexture) != null;

            var hasMatCap = mtoon.GetTexture(MToon10PropertyType.MatcapTexture) != null &&
                            mtoon.GetColor(MToon10PropertyType.MatcapColorFactor) != Color.black;

            var hasOutline = mtoon.GetInt(MToon10PropertyType.OutlineWidthMode) != (int)MToon10OutlineMode.None &&
                             mtoon.GetFloat(MToon10PropertyType.OutlineWidthFactor) > 0.0f;

            var alphaMode = mtoon.GetInt(MToon10PropertyType.AlphaMode);
            var transparentZWrite = mtoon.GetInt(MToon10PropertyType.TransparentWithZWrite) == (int)MToon10TransparentWithZWriteMode.On;
            var renderQueueOffset = mtoon.GetInt(MToon10PropertyType.RenderQueueOffsetNumber);

            // Rendering Mode
            var renderingMode = lilToonRenderingMode.Opaque;
            if (alphaMode == (int)MToon10AlphaMode.Cutout)
            {
                renderingMode = lilToonRenderingMode.Cutout;
            }
            else if (alphaMode == (int)MToon10AlphaMode.Transparent)
            {
                renderingMode = lilToonRenderingMode.Transparent;
            }

            var isOutline = hasOutline;
            var isCutout = alphaMode == (int)MToon10AlphaMode.Cutout;
            var isTransparent = alphaMode == (int)MToon10AlphaMode.Transparent;

            // lilToon shader selection
            var shaderName = "lilToon";
            if (isCutout) shaderName += "Cutout";
            if (isTransparent) shaderName += "Transparent";
            if (isOutline) shaderName += "Outline";
            if (shaderName != "lilToon") shaderName = "Hidden/" + shaderName;

            var shader = Shader.Find(shaderName);
            if (shader != null) liltoon.shader = shader;

            // Blend mode settings
            if (isTransparent)
            {
                liltoon.SetFloat("_SrcBlend", 1.0f);
                liltoon.SetFloat("_DstBlend", 10.0f);
                liltoon.SetFloat("_SrcBlendAlpha", 1.0f);
                liltoon.SetFloat("_DstBlendAlpha", 10.0f);
                liltoon.SetFloat("_BlendOp", 0.0f);
                liltoon.SetFloat("_BlendOpAlpha", 0.0f);
                liltoon.SetFloat("_SrcBlendFA", 1.0f);
                liltoon.SetFloat("_DstBlendFA", 1.0f);
                liltoon.SetFloat("_SrcBlendAlphaFA", 0.0f);
                liltoon.SetFloat("_DstBlendAlphaFA", 1.0f);
                liltoon.SetFloat("_BlendOpFA", 4.0f);
                liltoon.SetFloat("_BlendOpAlphaFA", 4.0f);
            }
            else
            {
                liltoon.SetFloat("_SrcBlend", 1.0f);
                liltoon.SetFloat("_DstBlend", 0.0f);
                liltoon.SetFloat("_SrcBlendAlpha", 1.0f);
                liltoon.SetFloat("_DstBlendAlpha", 10.0f);
                liltoon.SetFloat("_BlendOp", 0.0f);
                liltoon.SetFloat("_BlendOpAlpha", 0.0f);
                liltoon.SetFloat("_SrcBlendFA", 1.0f);
                liltoon.SetFloat("_DstBlendFA", 1.0f);
                liltoon.SetFloat("_SrcBlendAlphaFA", 0.0f);
                liltoon.SetFloat("_DstBlendAlphaFA", 1.0f);
                liltoon.SetFloat("_BlendOpFA", 4.0f);
                liltoon.SetFloat("_BlendOpAlphaFA", 4.0f);
            }

            if (isCutout)
            {
                liltoon.SetFloat("_AlphaToMask", 1.0f);
            }
            else
            {
                liltoon.SetFloat("_AlphaToMask", 0.0f);
            }

            // Base Settings
            liltoon.SetFloat(lilToonPropertyType.Cutoff, mtoon.GetFloat(MToon10PropertyType.AlphaCutoff));
            liltoon.SetFloat(lilToonPropertyType.ZWrite, !isTransparent ? 1.0f : (transparentZWrite ? 1.0f : 0.0f));

            // Cull Mode
            var doubleSided = mtoon.GetInt(MToon10PropertyType.DoubleSided);
            var cullMode = doubleSided == (int)MToon10DoubleSidedMode.On ? lilToonCullMode.Off : lilToonCullMode.Back;
            liltoon.SetInt(lilToonPropertyType.Cull, (int)cullMode);

            // Render Queue
            liltoon.renderQueue = renderingMode switch
            {
                lilToonRenderingMode.Opaque => (int)RenderQueue.Geometry + renderQueueOffset,
                lilToonRenderingMode.Cutout => (int)RenderQueue.AlphaTest + renderQueueOffset,
                lilToonRenderingMode.Transparent => transparentZWrite ? (int)RenderQueue.GeometryLast + 1 + renderQueueOffset
                                                                        : (int)RenderQueue.Transparent + renderQueueOffset,
                _ => throw new System.ArgumentOutOfRangeException(nameof(renderingMode), renderingMode, null),
            };

            // Main Color
            liltoon.SetColor(lilToonPropertyType.MainColor, mtoon.GetColor(MToon10PropertyType.BaseColorFactor));

            // Main Texture
            var baseColorTex = mtoon.GetTexture(MToon10PropertyType.BaseColorTexture);
            if (baseColorTex != null)
            {
                liltoon.SetTexture(lilToonPropertyType.MainTex, baseColorTex);
            }

            // Normal Map
            liltoon.SetFloat(lilToonPropertyType.UseNormalMap, hasNormalMap ? 1f : 0f);
            var normalTex = mtoon.GetTexture(MToon10PropertyType.NormalTexture);
            if (normalTex != null)
            {
                liltoon.SetTexture(lilToonPropertyType.NormalTex, normalTex);
                liltoon.SetFloat(lilToonPropertyType.NormalTexScale, mtoon.GetFloat(MToon10PropertyType.NormalTextureScale));
            }

            // Shadow
            liltoon.SetFloat(lilToonPropertyType.UseShadow, hasShadow ? 1f : 0f);
            liltoon.SetColor(lilToonPropertyType.ShadowColor, mtoon.GetColor(MToon10PropertyType.ShadeColorFactor));

            var shadeColorTex = mtoon.GetTexture(MToon10PropertyType.ShadeColorTexture);
            if (shadeColorTex != null)
            {
                liltoon.SetTexture(lilToonPropertyType.ShadowColorTex, shadeColorTex);
            }

            // Calculate ShadowBorder from ShadingShiftFactor.
            var shadingShiftFactor = mtoon.GetFloat(MToon10PropertyType.ShadingShiftFactor); // [-1.0, 1.0]
            var shadowBorder = Mathf.Clamp01(-0.5f * shadingShiftFactor + 0.5f); // [0.0, 1.0]

            // Calculate ShadowBlur from ShadingToonyFactor.
            var shadingToonyFactor = mtoon.GetFloat(MToon10PropertyType.ShadingToonyFactor); // [0.0, 1.0]
            var shadowBlur = Mathf.Clamp01(1.0f - shadingToonyFactor); // [0.0, 1.0]

            liltoon.SetFloat(lilToonPropertyType.ShadowBorder, shadowBorder);
            liltoon.SetFloat(lilToonPropertyType.ShadowBlur, shadowBlur);

            // Emission
            liltoon.SetFloat(lilToonPropertyType.UseEmission, hasEmission ? 1f : 0f);
            liltoon.SetColor(lilToonPropertyType.EmissionColor, mtoon.GetColor(MToon10PropertyType.EmissiveFactor));
            var emissiveTex = mtoon.GetTexture(MToon10PropertyType.EmissiveTexture);
            if (emissiveTex != null)
            {
                liltoon.SetTexture(lilToonPropertyType.EmissionTex, emissiveTex);
            }

            // Rim Light
            liltoon.SetFloat(lilToonPropertyType.UseRim, hasRimLight ? 1f : 0f);
            liltoon.SetColor(lilToonPropertyType.RimColor, mtoon.GetColor(MToon10PropertyType.ParametricRimColorFactor));

            var rimTex = mtoon.GetTexture(MToon10PropertyType.RimMultiplyTexture);
            if (rimTex != null)
            {
                liltoon.SetTexture(lilToonPropertyType.RimColorTex, rimTex);
            }

            liltoon.SetFloat(lilToonPropertyType.RimEnableLighting, mtoon.GetFloat(MToon10PropertyType.RimLightingMixFactor));

            var rimFresnelPower = mtoon.GetFloat(MToon10PropertyType.ParametricRimFresnelPowerFactor);
            liltoon.SetFloat(lilToonPropertyType.RimFresnelPower, rimFresnelPower);

            // Set default values for RimBorder and RimBlur, because it is difficult to calculate from MToon parameters.
            // MToonのパラメーターから計算することが困難なため、RimBorderとRimBlurの値はデフォルト値をセットする。
            var rimBorder = 0.5f;
            var rimBlur = 0.65f;

            liltoon.SetFloat(lilToonPropertyType.RimBorder, rimBorder);
            liltoon.SetFloat(lilToonPropertyType.RimBlur, rimBlur);

            // MatCap
            liltoon.SetFloat(lilToonPropertyType.UseMatCap, hasMatCap ? 1f : 0f);
            liltoon.SetColor(lilToonPropertyType.MatCapColor, mtoon.GetColor(MToon10PropertyType.MatcapColorFactor));
            var matcapTex = mtoon.GetTexture(MToon10PropertyType.MatcapTexture);
            if (matcapTex != null)
            {
                liltoon.SetTexture(lilToonPropertyType.MatCapTex, matcapTex);
            }

            // Outline
            liltoon.SetColor(lilToonPropertyType.OutlineColor, mtoon.GetColor(MToon10PropertyType.OutlineColorFactor));
            liltoon.SetTexture(lilToonPropertyType.OutlineTex, baseColorTex);

            var outlineWidthScaleFactor = 100f; // The outline width of lilToon is approximately 100 times that of MToon10.
            liltoon.SetFloat(lilToonPropertyType.OutlineWidth, mtoon.GetFloat(MToon10PropertyType.OutlineWidthFactor) * outlineWidthScaleFactor);

            var outlineWidthTex = mtoon.GetTexture(MToon10PropertyType.OutlineWidthMultiplyTexture);
            if (outlineWidthTex != null)
            {
                liltoon.SetTexture(lilToonPropertyType.OutlineWidthMask, outlineWidthTex);
            }
        }
    }
}
