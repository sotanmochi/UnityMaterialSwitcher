using System.Collections.Generic;

// namespace VRM10.MToon10
namespace MaterialSwitcher
{
    public static class MToon10Properties
    {
        private static readonly Dictionary<MToon10PropertyType, string> _unityShaderLabNames = new Dictionary<MToon10PropertyType, string>
        {
            [MToon10PropertyType.AlphaMode] = "_AlphaMode",
            [MToon10PropertyType.TransparentWithZWrite] = "_TransparentWithZWrite",
            [MToon10PropertyType.AlphaCutoff] = "_Cutoff",
            [MToon10PropertyType.RenderQueueOffsetNumber] = "_RenderQueueOffset",
            [MToon10PropertyType.DoubleSided] = "_DoubleSided",

            [MToon10PropertyType.BaseColorFactor] = "_Color",
            [MToon10PropertyType.BaseColorTexture] = "_MainTex",
            [MToon10PropertyType.ShadeColorFactor] = "_ShadeColor",
            [MToon10PropertyType.ShadeColorTexture] = "_ShadeTex",
            [MToon10PropertyType.NormalTexture] = "_BumpMap",
            [MToon10PropertyType.NormalTextureScale] = "_BumpScale",
            [MToon10PropertyType.ShadingShiftFactor] = "_ShadingShiftFactor",
            [MToon10PropertyType.ShadingShiftTexture] = "_ShadingShiftTex",
            [MToon10PropertyType.ShadingShiftTextureScale] = "_ShadingShiftTexScale",
            [MToon10PropertyType.ShadingToonyFactor] = "_ShadingToonyFactor",

            [MToon10PropertyType.GiEqualizationFactor] = "_GiEqualization",

            [MToon10PropertyType.EmissiveFactor] = "_EmissionColor",
            [MToon10PropertyType.EmissiveTexture] = "_EmissionMap",

            [MToon10PropertyType.MatcapColorFactor] = "_MatcapColor",
            [MToon10PropertyType.MatcapTexture] = "_MatcapTex",
            [MToon10PropertyType.ParametricRimColorFactor] = "_RimColor",
            [MToon10PropertyType.ParametricRimFresnelPowerFactor] = "_RimFresnelPower",
            [MToon10PropertyType.ParametricRimLiftFactor] = "_RimLift",
            [MToon10PropertyType.RimMultiplyTexture] = "_RimTex",
            [MToon10PropertyType.RimLightingMixFactor] = "_RimLightingMix",

            [MToon10PropertyType.OutlineWidthMode] = "_OutlineWidthMode",
            [MToon10PropertyType.OutlineWidthFactor] = "_OutlineWidth",
            [MToon10PropertyType.OutlineWidthMultiplyTexture] = "_OutlineWidthTex",
            [MToon10PropertyType.OutlineColorFactor] = "_OutlineColor",
            [MToon10PropertyType.OutlineLightingMixFactor] = "_OutlineLightingMix",

            [MToon10PropertyType.UvAnimationMaskTexture] = "_UvAnimMaskTex",
            [MToon10PropertyType.UvAnimationScrollXSpeedFactor] = "_UvAnimScrollXSpeed",
            [MToon10PropertyType.UvAnimationScrollYSpeedFactor] = "_UvAnimScrollYSpeed",
            [MToon10PropertyType.UvAnimationRotationSpeedFactor] = "_UvAnimRotationSpeed",

            [MToon10PropertyType.UnityCullMode] = "_M_CullMode",
            [MToon10PropertyType.UnitySrcBlend] = "_M_SrcBlend",
            [MToon10PropertyType.UnityDstBlend] = "_M_DstBlend",
            [MToon10PropertyType.UnityZWrite] = "_M_ZWrite",
            [MToon10PropertyType.UnityAlphaToMask] = "_M_AlphaToMask",
            
            [MToon10PropertyType.EditorEditMode] = "_M_EditMode",
        };

        public static IReadOnlyDictionary<MToon10PropertyType, string> UnityShaderLabNames => _unityShaderLabNames;

        public static string ToUnityShaderLabName(this MToon10PropertyType prop)
        {
            return UnityShaderLabNames[prop];
        }
    }
}