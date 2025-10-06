using System.Collections.Generic;

namespace MaterialSwitcher
{
    public static class lilToonProperties
    {
        private static readonly Dictionary<lilToonPropertyType, string> _unityShaderLabNames = new Dictionary<lilToonPropertyType, string>
        {
            [lilToonPropertyType.RenderingMode] = "_TransparentMode",

            // Base Settings
            [lilToonPropertyType.Cutoff] = "_Cutoff",
            [lilToonPropertyType.Cull] = "_Cull",
            [lilToonPropertyType.Invisible] = "_Invisible",
            [lilToonPropertyType.ZWrite] = "_ZWrite",
            [lilToonPropertyType.AntiAliasStrength] = "_AAStrength",
            [lilToonPropertyType.UseDither] = "_UseDither",
            [lilToonPropertyType.DitherTex] = "_DitherTex",
            [lilToonPropertyType.DitherMaxValue] = "_DitherMaxValue",

            // Lighting Base Settings
            [lilToonPropertyType.LightMinLimit] = "_LightMinLimit",
            [lilToonPropertyType.LightMaxLimit] = "_LightMaxLimit",
            [lilToonPropertyType.MonochromeLighting] = "_MonochromeLighting",
            [lilToonPropertyType.ShadowEnvStrength] = "_ShadowEnvStrength",

            // Main Color
            [lilToonPropertyType.MainColor] = "_Color",
            [lilToonPropertyType.MainTex] = "_MainTex",
            [lilToonPropertyType.MainTexHSVG] = "_MainTexHSVG",
            [lilToonPropertyType.MainGradationStrength] = "_MainGradationStrength",
            [lilToonPropertyType.MainGradationTex] = "_MainGradationTex",
            [lilToonPropertyType.MainColorAdjustMask] = "_MainColorAdjustMask",

            // Alpha Mask
            [lilToonPropertyType.AlphaMaskMode] = "_AlphaMaskMode",
            [lilToonPropertyType.AlphaMask] = "_AlphaMask",
            [lilToonPropertyType.AlphaMaskScale] = "_AlphaMaskScale",
            [lilToonPropertyType.AlphaMaskValue] = "_AlphaMaskValue",

            // Shadow
            [lilToonPropertyType.UseShadow] = "_UseShadow",
            [lilToonPropertyType.ShadowMaskType] = "_ShadowMaskType",
            [lilToonPropertyType.ShadowStrength] = "_ShadowStrength",
            [lilToonPropertyType.ShadowStrengthMask] = "_ShadowStrengthMask",
            [lilToonPropertyType.ShadowStrengthMaskLOD] = "_ShadowStrengthMaskLOD",
            [lilToonPropertyType.ShadowFlatBorder] = "_ShadowFlatBorder",
            [lilToonPropertyType.ShadowFlatBlur] = "_ShadowFlatBlur",
            [lilToonPropertyType.ShadowColorType] = "_ShadowColorType",
            [lilToonPropertyType.ShadowColor] = "_ShadowColor",
            [lilToonPropertyType.ShadowColorTex] = "_ShadowColorTex",
            [lilToonPropertyType.ShadowBorder] = "_ShadowBorder",
            [lilToonPropertyType.ShadowBlur] = "_ShadowBlur",
            [lilToonPropertyType.ShadowReceive] = "_ShadowReceive",
            [lilToonPropertyType.Shadow2ndColor] = "_Shadow2ndColor",
            [lilToonPropertyType.Shadow2ndColorTex] = "_Shadow2ndColorTex",
            [lilToonPropertyType.Shadow2ndBorder] = "_Shadow2ndBorder",
            [lilToonPropertyType.Shadow2ndBlur] = "_Shadow2ndBlur",
            [lilToonPropertyType.Shadow2ndReceive] = "_Shadow2ndReceive",
            [lilToonPropertyType.Shadow3rdColor] = "_Shadow3rdColor",
            [lilToonPropertyType.Shadow3rdColorTex] = "_Shadow3rdColorTex",
            [lilToonPropertyType.ShadowBorderColor] = "_ShadowBorderColor",
            [lilToonPropertyType.ShadowBorderRange] = "_ShadowBorderRange",
            [lilToonPropertyType.ShadowMainStrength] = "_ShadowMainStrength",

            // Emission
            [lilToonPropertyType.UseEmission] = "_UseEmission",
            [lilToonPropertyType.EmissionColor] = "_EmissionColor",
            [lilToonPropertyType.EmissionTex] = "_EmissionMap",
            [lilToonPropertyType.EmissionTexScrollRotate] = "_EmissionMap_ScrollRotate",
            [lilToonPropertyType.EmissionTexUVMode] = "_EmissionMap_UVMode",
            [lilToonPropertyType.EmissionMainStrength] = "_EmissionMainStrength",
            [lilToonPropertyType.EmissionBlend] = "_EmissionBlend",
            [lilToonPropertyType.EmissionBlendMask] = "_EmissionBlendMask",
            [lilToonPropertyType.EmissionBlendMaskScrollRotate] = "_EmissionBlendMask_ScrollRotate",
            [lilToonPropertyType.EmissionBlendMode] = "_EmissionBlendMode",
            [lilToonPropertyType.EmissionFluorescence] = "_EmissionFluorescence",
            [lilToonPropertyType.UseEmission2nd] = "_UseEmission2nd",
            [lilToonPropertyType.Emission2ndColor] = "_Emission2ndColor",
            [lilToonPropertyType.Emission2ndTex] = "_Emission2ndMap",
            [lilToonPropertyType.Emission2ndTexScrollRotate] = "_Emission2ndMap_ScrollRotate",
            [lilToonPropertyType.Emission2ndTexUVMode] = "_Emission2ndMap_UVMode",
            [lilToonPropertyType.Emission2ndMainStrength] = "_Emission2ndMainStrength",
            [lilToonPropertyType.Emission2ndBlend] = "_Emission2ndBlend",
            [lilToonPropertyType.Emission2ndBlendMask] = "_Emission2ndBlendMask",
            [lilToonPropertyType.Emission2ndBlendMaskScrollRotate] = "_Emission2ndBlendMask_ScrollRotate",
            [lilToonPropertyType.Emission2ndBlendMode] = "_Emission2ndBlendMode",
            [lilToonPropertyType.Emission2ndFluorescence] = "_Emission2ndFluorescence",

            // Normal Map
            [lilToonPropertyType.UseNormalMap] = "_UseBumpMap",
            [lilToonPropertyType.NormalTex] = "_BumpMap",
            [lilToonPropertyType.NormalTexScale] = "_BumpScale",
            [lilToonPropertyType.UseNormal2ndMap] = "_UseBump2ndMap",
            [lilToonPropertyType.Normal2ndTex] = "_Bump2ndMap",
            [lilToonPropertyType.Normal2ndTexUVMode] = "_Bump2ndMap_UVMode",
            [lilToonPropertyType.Normal2ndTexScale] = "_Bump2ndScale",
            [lilToonPropertyType.Normal2ndTexScaleMask] = "_Bump2ndScaleMask",

            // MatCap
            [lilToonPropertyType.UseMatCap] = "_UseMatCap",
            [lilToonPropertyType.MatCapColor] = "_MatCapColor",
            [lilToonPropertyType.MatCapTex] = "_MatCapTex",
            [lilToonPropertyType.MatCapBlendUV1] = "_MatCapBlendUV1",
            [lilToonPropertyType.MatCapZRotCancel] = "_MatCapZRotCancel",
            [lilToonPropertyType.MatCapPerspective] = "_MatCapPerspective",
            [lilToonPropertyType.MatCapVRParallaxStrength] = "_MatCapVRParallaxStrength",
            [lilToonPropertyType.MatCapBlend] = "_MatCapBlend",
            [lilToonPropertyType.MatCapBlendMask] = "_MatCapBlendMask",
            [lilToonPropertyType.MatCapEnableLighting] = "_MatCapEnableLighting",
            [lilToonPropertyType.MatCapBlendMode] = "_MatCapBlendMode",
            [lilToonPropertyType.UseMatCap2nd] = "_UseMatCap2nd",
            [lilToonPropertyType.MatCap2ndColor] = "_MatCap2ndColor",
            [lilToonPropertyType.MatCap2ndTex] = "_MatCap2ndTex",
            [lilToonPropertyType.MatCap2ndBlendUV1] = "_MatCap2ndBlendUV1",
            [lilToonPropertyType.MatCap2ndZRotCancel] = "_MatCap2ndZRotCancel",
            [lilToonPropertyType.MatCap2ndPerspective] = "_MatCap2ndPerspective",
            [lilToonPropertyType.MatCap2ndVRParallaxStrength] = "_MatCap2ndVRParallaxStrength",
            [lilToonPropertyType.MatCap2ndBlend] = "_MatCap2ndBlend",
            [lilToonPropertyType.MatCap2ndBlendMask] = "_MatCap2ndBlendMask",
            [lilToonPropertyType.MatCap2ndEnableLighting] = "_MatCap2ndEnableLighting",
            [lilToonPropertyType.MatCap2ndBlendMode] = "_MatCap2ndBlendMode",

            // Rim Light
            [lilToonPropertyType.UseRim] = "_UseRim",
            [lilToonPropertyType.RimColor] = "_RimColor",
            [lilToonPropertyType.RimColorTex] = "_RimColorTex",
            [lilToonPropertyType.RimBorder] = "_RimBorder",
            [lilToonPropertyType.RimBlur] = "_RimBlur",
            [lilToonPropertyType.RimFresnelPower] = "_RimFresnelPower",
            [lilToonPropertyType.RimEnableLighting] = "_RimEnableLighting",
            [lilToonPropertyType.RimDirStrength] = "_RimDirStrength",
            [lilToonPropertyType.RimDirRange] = "_RimDirRange",
            [lilToonPropertyType.RimIndirColor] = "_RimIndirColor",
            [lilToonPropertyType.RimIndirBorder] = "_RimIndirBorder",
            [lilToonPropertyType.RimIndirBlur] = "_RimIndirBlur",
            [lilToonPropertyType.RimIndirRange] = "_RimIndirRange",
            [lilToonPropertyType.RimBlendMode] = "_RimBlendMode",

            // Outline
            [lilToonPropertyType.OutlineColor] = "_OutlineColor",
            [lilToonPropertyType.OutlineTex] = "_OutlineTex",
            [lilToonPropertyType.OutlineTexScrollRotate] = "_OutlineTex_ScrollRotate",
            [lilToonPropertyType.OutlineWidth] = "_OutlineWidth",
            [lilToonPropertyType.OutlineWidthMask] = "_OutlineWidthMask",
        };

        public static IReadOnlyDictionary<lilToonPropertyType, string> UnityShaderLabNames => _unityShaderLabNames;

        public static string ToUnityShaderLabName(this lilToonPropertyType prop)
        {
            return UnityShaderLabNames[prop];
        }
    }
}