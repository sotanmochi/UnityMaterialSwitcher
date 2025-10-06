using UnityEngine;

namespace MaterialSwitcher
{
    public static partial class MaterialExtensions
    {
        #region Getters

        public static int GetInt(this Material mat, MToon10PropertyType prop)
        {
            return mat.GetInt(prop.ToUnityShaderLabName());
        }

        public static Texture GetTexture(this Material mat, MToon10PropertyType prop)
        {
            return mat.GetTexture(prop.ToUnityShaderLabName());
        }

        public static float GetFloat(this Material mat, MToon10PropertyType prop)
        {
            return mat.GetFloat(prop.ToUnityShaderLabName());
        }

        public static Color GetColor(this Material mat, MToon10PropertyType prop)
        {
            return mat.GetColor(prop.ToUnityShaderLabName());
        }

        #endregion

        #region Setters

        public static void SetInt(this Material mat, MToon10PropertyType prop, int val)
        {
            mat.SetInt(prop.ToUnityShaderLabName(), val);
        }

        public static void SetTexture(this Material mat, MToon10PropertyType prop, Texture val)
        {
            mat.SetTexture(prop.ToUnityShaderLabName(), val);
        }

        public static void SetFloat(this Material mat, MToon10PropertyType prop, float val)
        {
            mat.SetFloat(prop.ToUnityShaderLabName(), val);
        }

        public static void SetColor(this Material mat, MToon10PropertyType prop, Color val)
        {
            mat.SetColor(prop.ToUnityShaderLabName(), val);
        }

        #endregion
    }
}