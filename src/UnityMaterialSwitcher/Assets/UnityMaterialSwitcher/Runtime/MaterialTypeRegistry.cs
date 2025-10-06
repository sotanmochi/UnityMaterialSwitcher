using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MaterialSwitcher
{
    public sealed class MaterialTypeRegistry
    {
        private readonly Dictionary<string, IMaterialReferenceProvider> _providers = new();
        private readonly Dictionary<string, IMaterialConverter> _converters = new();

        public void RegisterMaterialReferenceProvider(IMaterialReferenceProvider provider)
        {
            if (provider == null)
            {
                UnityLogger.LogError($"[{nameof(MaterialTypeRegistry)}] Cannot register null provider.");
                return;
            }

            if (_providers.ContainsKey(provider.MaterialTypeId))
            {
                UnityLogger.Log($"<color=orange>[{nameof(MaterialTypeRegistry)}] Provider for '{provider.MaterialTypeId}' is already registered. Overwriting.</color>");
            }

            _providers[provider.MaterialTypeId] = provider;
            UnityLogger.Log($"[{nameof(MaterialTypeRegistry)}] Registered reference provider: {provider.MaterialTypeId}");
        }

        public void RegisterConverter(IMaterialConverter converter)
        {
            if (converter == null)
            {
                UnityLogger.LogError($"[{nameof(MaterialTypeRegistry)}] Cannot register null converter.");
                return;
            }

            var key = GetConverterKey(converter.SourceMaterialTypeId, converter.TargetMaterialTypeId);

            if (_converters.ContainsKey(key))
            {
                UnityLogger.Log($"<color=orange>[{nameof(MaterialTypeRegistry)}] Converter for '{key}' is already registered. Overwriting.</color>");
            }

            _converters[key] = converter;
            UnityLogger.Log($"[{nameof(MaterialTypeRegistry)}] Registered converter: {converter.SourceMaterialTypeId} -> {converter.TargetMaterialTypeId}");
        }

        public IMaterialReferenceProvider GetMaterialReferenceProvider(string materialTypeId)
        {
            if (_providers.TryGetValue(materialTypeId, out var provider))
            {
                return provider;
            }

            UnityLogger.LogError($"[{nameof(MaterialTypeRegistry)}] Reference provider is not found for material type: {materialTypeId}");
            return null;
        }

        public IMaterialReferenceProvider GetMaterialReferenceProvider(Material material)
        {
            if (material == null)
            {
                UnityLogger.LogError($"[{nameof(MaterialTypeRegistry)}] Cannot get reference provider for null material.");
                return null;
            }

            return _providers.Values.FirstOrDefault(provider => provider.IsMatch(material));
        }

        public IMaterialConverter GetConverter(string sourceMaterialTypeId, string targetMaterialTypeId)
        {
            var key = GetConverterKey(sourceMaterialTypeId, targetMaterialTypeId);

            if (_converters.TryGetValue(key, out var converter))
            {
                return converter;
            }

            UnityLogger.LogError($"[{nameof(MaterialTypeRegistry)}] Material converter is not found: {sourceMaterialTypeId} -> {targetMaterialTypeId}");
            return null;
        }

        public IMaterialConverter GetConverter(Material sourceMaterial, Material targetMaterial)
        {
            if (sourceMaterial == null || targetMaterial == null)
            {
                UnityLogger.LogError($"[{nameof(MaterialTypeRegistry)}] Cannot get converter for null materials.");
                return null;
            }

            var sourceHandler = GetMaterialReferenceProvider(sourceMaterial);
            var targetHandler = GetMaterialReferenceProvider(targetMaterial);
            if (sourceHandler == null || targetHandler == null)
            {
                return null;
            }

            return GetConverter(sourceHandler.MaterialTypeId, targetHandler.MaterialTypeId);
        }

        private string GetConverterKey(string sourceTypeId, string targetTypeId)
        {
            return $"{sourceTypeId}->{targetTypeId}";
        }
    }
}
