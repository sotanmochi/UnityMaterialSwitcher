using System.Collections.Generic;
using UnityEngine;

namespace MaterialSwitcher
{
    public abstract class MaterialReference : MonoBehaviour
    {
        [SerializeField, ReadOnly] private List<Material> _materials = new();

        public abstract string GetShaderName();

        public void SetMaterial(int index, Material material)
        {
            if (index < _materials.Count)
            {
                _materials[index] = material;
                return;
            }

            while (_materials.Count < index + 1)
            {
                _materials.Add(null);
            }
            _materials[index] = material;
        }

        public void GetOrAddMaterial(int index, out Material material, out bool isNew)
        {
            isNew = false;
            if (index < 0)
            {
                material = null;
                return;
            }

            if (index < _materials.Count)
            {
                if (_materials[index] == null)
                {
                    _materials[index] = new Material(Shader.Find(GetShaderName()));
                    isNew = true;
                }

                material = _materials[index];
                return;
            }

            while (_materials.Count < index + 1)
            {
                _materials.Add(null);
            }
            _materials[index] = new Material(Shader.Find(GetShaderName()));
            isNew = true;
            material = _materials[index];
        }
    }
}