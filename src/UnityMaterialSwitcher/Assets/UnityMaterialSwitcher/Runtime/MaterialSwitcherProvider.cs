namespace MaterialSwitcher
{
    public static class MaterialSwitcherProvider
    {
        private static MaterialSwitcher _instance;

        public static MaterialSwitcher Get()
        {
            if (_instance == null)
            {
                SetupDefaultMaterialSwitcher();
            }
            return _instance;
        }

        public static void Set(MaterialSwitcher service)
        {
            _instance = service;
        }

        public static void SetupDefaultMaterialSwitcher()
        {
            var registry = new MaterialTypeRegistry();

            registry.RegisterMaterialReferenceProvider(new MToon10ReferenceProvider());
            registry.RegisterMaterialReferenceProvider(new lilToonReferenceProvider());

            registry.RegisterConverter(new MToonToLilToonConverter());
            registry.RegisterConverter(new LilToonToMToonConverter());

            _instance = new MaterialSwitcher(registry);

            UnityLogger.Log($"<color=cyan>[{nameof(MaterialSwitcherProvider)}] Default material switcher is set up.</color>");
        }
    }
}
