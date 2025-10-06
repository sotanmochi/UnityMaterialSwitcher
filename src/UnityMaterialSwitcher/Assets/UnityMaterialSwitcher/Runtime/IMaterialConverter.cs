using UnityEngine;

namespace MaterialSwitcher
{
    public interface IMaterialConverter
    {
        string SourceMaterialTypeId { get; }
        string TargetMaterialTypeId { get; }
        bool CanConvert(Material sourceMaterial, Material targetMaterial);
        void Convert(Material sourceMaterial, Material targetMaterial);
    }
}
