using UnityEngine;

namespace MaterialSwitcher
{
    public interface IMaterialReferenceProvider
    {
        string MaterialTypeId { get; }
        bool IsMatch(Material material);
        MaterialReference GetOrAdd(GameObject gameObject);
    }
}
