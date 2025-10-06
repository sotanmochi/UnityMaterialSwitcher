using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MaterialSwitcher
{
    public sealed class MaterialSwitcher
    {
        private readonly MaterialTypeRegistry _registry;

        public MaterialSwitcher(MaterialTypeRegistry registry)
        {
            _registry = registry ?? throw new System.ArgumentNullException(nameof(registry));
        }

        /// <summary>
        /// Switch all materials in the target object to the specified material type. <br/>
        /// If the corresponding material does not exist, a new material will be created and assigned. <br/>
        /// The conversion process from the current material to the target material will be executed 
        /// only when the target material is newly created. <br/>
        /// 
        /// ターゲットオブジェクト内の全てのマテリアルを指定したマテリアルタイプに切り替えます。<br/>
        /// 対応するマテリアルが存在しない場合は、マテリアルが新規作成されて割り当てられます。<br/>
        /// 切り替え先のマテリアルが新規作成された時のみ、現在のマテリアルからのコンバート処理を実行します。<br/>
        /// 
        /// </summary>
        /// <param name="targetObject"></param>
        /// <param name="targetMaterialTypeId"></param>
        public void SwitchMaterials(GameObject targetObject, string targetMaterialTypeId)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
#endif

            if (targetObject == null)
            {
                UnityLogger.LogError($"[{nameof(MaterialSwitcher)}] Target object is null.");
                return;
            }

            var targetMaterialReferenceProvider = _registry.GetMaterialReferenceProvider(targetMaterialTypeId);
            if (targetMaterialReferenceProvider == null)
            {
                UnityLogger.LogError($"[{nameof(MaterialSwitcher)}] MaterialReferenceProvider is not found for material type: {targetMaterialTypeId}");
                return;
            }

            UnityLogger.Log($"<color=cyan>[{nameof(MaterialSwitcher)}] Change materials to {targetMaterialTypeId}.</color>");

            var renderers = targetObject.GetComponentsInChildren<Renderer>(includeInactive: true);
            foreach (var renderer in renderers)
            {
                var materialReference = targetMaterialReferenceProvider.GetOrAdd(renderer.gameObject);

                var materials = new Material[renderer.sharedMaterials.Length];
                for (var index = 0; index < renderer.sharedMaterials.Length; index++)
                {
                    var currentMaterial = renderer.sharedMaterials[index];
                    if (currentMaterial == null) continue;

                    Material targetMaterial = null;
                    bool isNewMaterial = false;

                    if (targetMaterialReferenceProvider.IsMatch(currentMaterial))
                    {
                        materialReference.SetMaterial(index, currentMaterial);
                        targetMaterial = currentMaterial;
                    }
                    else
                    {
                        materialReference.GetOrAddMaterial(index, out targetMaterial, out isNewMaterial);

                        if (isNewMaterial)
                        {
                            targetMaterial.name = $"{currentMaterial.name}_{targetMaterialTypeId}";

#if UNITY_EDITOR
                            var directoryPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(currentMaterial));
                            directoryPath = Path.Combine(directoryPath, $"{targetObject.name}.Materials.{targetMaterialTypeId}");
                            Directory.CreateDirectory(directoryPath);

                            var assetPath = Path.Combine(directoryPath, $"{targetMaterial.name}.mat");
                            AssetDatabase.CreateAsset(targetMaterial, assetPath);
                            UnityLogger.Log($"[{nameof(MaterialSwitcher)}] Create new asset: {assetPath}");
#endif
                        }
                    }

                    if (isNewMaterial)
                    {
                        // Execute the conversion process from the current material only when the target material is newly created.
                        // 切り替え先のマテリアルが新規作成された時のみ、現在のマテリアルからのコンバート処理を実行する。
                        ConvertMaterial(currentMaterial, targetMaterial);
                    }

                    UpdateMaterialReference(renderer, index, currentMaterial);
                    materials[index] = targetMaterial;
                }

#if UNITY_EDITOR
                Undo.IncrementCurrentGroup();
                Undo.RecordObject(renderer, $"Change materials to {targetMaterialTypeId} ({renderer.gameObject.name}).");
#endif
                renderer.sharedMaterials = materials;
            }

            UnityLogger.Log($"<color=cyan>[{nameof(MaterialSwitcher)}] Material switching completed.</color>");

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            stopwatch.Stop();
            UnityLogger.Log($"[{nameof(MaterialSwitcher)}] Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
#endif
        }

        private void UpdateMaterialReference(Renderer renderer, int materialIndex, Material currentMaterial)
        {
            var referenceProvider = _registry.GetMaterialReferenceProvider(currentMaterial);
            if (referenceProvider != null)
            {
                var materialReference = referenceProvider.GetOrAdd(renderer.gameObject);
                materialReference.SetMaterial(materialIndex, currentMaterial);
            }
        }

        private void ConvertMaterial(Material sourceMaterial, Material targetMaterial)
        {
            var converter = _registry.GetConverter(sourceMaterial, targetMaterial);
            if (converter != null && converter.CanConvert(sourceMaterial, targetMaterial))
            {
                converter.Convert(sourceMaterial, targetMaterial);
            }
            else
            {
                UnityLogger.Log($"<color=yellow>[{nameof(MaterialSwitcher)}] No suitable converter found for {sourceMaterial.name} -> {targetMaterial.name}</color>");
            }
        }
    }
}
