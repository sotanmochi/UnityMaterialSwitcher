#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace MaterialSwitcher
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out var component))
            {
#if UNITY_EDITOR
                Undo.IncrementCurrentGroup();
                component = Undo.AddComponent<T>(gameObject);
#else
                component = gameObject.AddComponent<T>();
#endif
            }
            return component;
        }
    }
}