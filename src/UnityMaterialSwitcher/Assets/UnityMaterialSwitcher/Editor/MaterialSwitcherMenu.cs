using UnityEditor;

namespace MaterialSwitcher.Editor
{
    public static class MaterialSwitcherMenu
    {
        [MenuItem("Material Switcher/Setup Default Material Switcher", false)]
        public static void SetupDefaultMaterialSwitcher()
        {
            MaterialSwitcherProvider.SetupDefaultMaterialSwitcher();
            EditorUtility.DisplayDialog("Material Switcher", "Default Material Switcher has been set up.", "OK");
        }

        [MenuItem("GameObject/Material Switcher/Switch to MToon10", false)]
        public static void SwitchToMToon10()
        {
            var materialSwitcher = MaterialSwitcherProvider.Get();
            materialSwitcher.SwitchMaterials(Selection.activeGameObject, "MToon10");
        }

        [MenuItem("GameObject/Material Switcher/Switch to lilToon", false)]
        public static void SwitchToLilToon()
        {
            var materialSwitcher = MaterialSwitcherProvider.Get();
            materialSwitcher.SwitchMaterials(Selection.activeGameObject, "lilToon");
        }
    }
}
