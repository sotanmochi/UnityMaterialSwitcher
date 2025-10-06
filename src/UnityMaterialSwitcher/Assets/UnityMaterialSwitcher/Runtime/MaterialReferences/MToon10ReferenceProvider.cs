#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace MaterialSwitcher
{
    public sealed class MToon10ReferenceProvider : IMaterialReferenceProvider
    {
        public string MaterialTypeId => "MToon10";

        public bool IsMatch(Material material)
        {
            if (material == null || material.shader == null)
            {
                return false;
            }

            var currentRenderPipeline = RenderPipelineUtility.GetRenderPipelineType();
            var shaderName = currentRenderPipeline switch
            {
                RenderPipelineType.UniversalRenderPipeline => "VRM10/Universal Render Pipeline/MToon10",
                _ => "VRM10/MToon10"
            };

            return material.shader.name.Contains(shaderName);
        }

        public MaterialReference GetOrAdd(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<MToon10Reference>(out var component))
            {
#if UNITY_EDITOR
                Undo.IncrementCurrentGroup();
                component = Undo.AddComponent<MToon10Reference>(gameObject);
#else
                component = gameObject.AddComponent<MToon10Reference>();
#endif
            }
            return component;
        }
    }
}
