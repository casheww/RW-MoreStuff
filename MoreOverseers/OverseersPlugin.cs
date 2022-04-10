using System;
using BepInEx;

namespace MoreOverseers
{
    [BepInPlugin("casheww.more_overseers", "MoreOverseers", "0.1.0")]
    public class OverseersPlugin : BaseUnityPlugin
    {
        public OverseersPlugin()
        {
            Instance = this;
        }

        public static OverseersPlugin Instance { get; private set; }
        public static BepInEx.Logging.ManualLogSource Logger_ => Instance.Logger;
        public static OptionalUI.OptionInterface LoadOI() => new ConfigMenu();


        void OnEnable()
        {
            SpawnHooks.Apply();
            BehaveHooks.Apply();
            GraphicsHooks.Apply();
            Interactions.ConversationHooks.Apply();
        }
        void OnDisable()
        {
            SpawnHooks.UnApply();
            BehaveHooks.UnApply();
            GraphicsHooks.UnApply();
            Interactions.ConversationHooks.UnApply();
        }

    }
}
