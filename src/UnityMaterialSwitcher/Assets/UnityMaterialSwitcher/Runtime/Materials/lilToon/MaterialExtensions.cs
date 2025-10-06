using UnityEngine;

namespace MaterialSwitcher
{
    public static partial class MaterialExtensions
    {
        #region Getters

        public static int GetInt(this Material mat, lilToonPropertyType prop)
        {
            return mat.GetInt(prop.ToUnityShaderLabName());
        }

        public static Texture GetTexture(this Material mat, lilToonPropertyType prop)
        {
            return mat.GetTexture(prop.ToUnityShaderLabName());
        }

        public static float GetFloat(this Material mat, lilToonPropertyType prop)
        {
            return mat.GetFloat(prop.ToUnityShaderLabName());
        }

        public static Color GetColor(this Material mat, lilToonPropertyType prop)
        {
            return mat.GetColor(prop.ToUnityShaderLabName());
        }

        #endregion

        #region Setters

        public static void SetInt(this Material mat, lilToonPropertyType prop, int val)
        {
            mat.SetInt(prop.ToUnityShaderLabName(), val);
        }

        public static void SetTexture(this Material mat, lilToonPropertyType prop, Texture val)
        {
            mat.SetTexture(prop.ToUnityShaderLabName(), val);
        }

        public static void SetFloat(this Material mat, lilToonPropertyType prop, float val)
        {
            mat.SetFloat(prop.ToUnityShaderLabName(), val);
        }

        public static void SetColor(this Material mat, lilToonPropertyType prop, Color val)
        {
            mat.SetColor(prop.ToUnityShaderLabName(), val);
        }

        #endregion
    }
}