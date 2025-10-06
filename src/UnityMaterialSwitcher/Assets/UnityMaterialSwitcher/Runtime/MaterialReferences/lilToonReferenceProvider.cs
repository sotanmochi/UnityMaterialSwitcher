#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace MaterialSwitcher
{
    public sealed class lilToonReferenceProvider : IMaterialReferenceProvider
    {
        public string MaterialTypeId => "lilToon";

        public bool IsMatch(Material material)
        {
            if (material == null || material.shader == null)
            {
                return false;
            }

            return material.shader.name.Contains("lilToon");
        }

        public MaterialReference GetOrAdd(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<lilToonReference>(out var component))
            {
#if UNITY_EDITOR
                Undo.IncrementCurrentGroup();
                component = Undo.AddComponent<lilToonReference>(gameObject);
#else
                component = gameObject.AddComponent<lilToonReference>();
#endif
            }
            return component;
        }
    }
}
