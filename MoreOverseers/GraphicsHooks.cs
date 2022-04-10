using System.Collections.Generic;
using System.Reflection;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace MoreOverseers
{
    class GraphicsHooks
    {
        public static void Apply()
        {
            overseerColorHook = new Hook(
                typeof(OverseerGraphics).GetProperty("MainColor", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(),
                typeof(GraphicsHooks).GetMethod("OverseerGraphics_get_MainColor", BindingFlags.Static | BindingFlags.Public));
        }
        public static void UnApply()
        {
            overseerColorHook.Undo();
        }

        static Hook overseerColorHook;

        public delegate Color orig_MainColor(OverseerGraphics self);

        public static Color OverseerGraphics_get_MainColor(orig_MainColor orig, OverseerGraphics self)
        {
            Color color;

            switch (ConfigMenu.ColourMode_)
            {
                case ConfigMenu.ColourMode.Default:
                    color = orig(self);
                    break;

                case ConfigMenu.ColourMode.RandomStrobe:
                    color = new HSLColor(Random.value, 1, 0.8f).rgb;
                    break;

                case ConfigMenu.ColourMode.RandomStatic:
                    if (staticOverseerColours.ContainsKey(self.owner.abstractPhysicalObject.ID))
                    {
                        color = staticOverseerColours[self.owner.abstractPhysicalObject.ID];
                    }
                    else
                    {
                        color = new HSLColor(Random.value, 1, 0.8f).rgb;
                        staticOverseerColours[self.owner.abstractPhysicalObject.ID] = color;
                    }
                    break;

                case ConfigMenu.ColourMode.AllMoon:
                    color = new Color(1f, 0.8f, 0.3f);
                    break;

                default:
                case ConfigMenu.ColourMode.AllPebbles:
                    color = new Color(0.447058827f, 0.9019608f, 0.768627465f);
                    break;

                case ConfigMenu.ColourMode.AllGreenOverseerFromUnknownIterator:
                    color = new Color(0f, 1f, 0f);
                    break;

                case ConfigMenu.ColourMode.Custom:
                    color = ConfigMenu.CustomColour;
                    break;

            }
            
            return color;
        }

        static Dictionary<EntityID, Color> staticOverseerColours = new Dictionary<EntityID, Color>();

    }
}
