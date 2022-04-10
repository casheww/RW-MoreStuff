using UnityEngine;

namespace MoreOverseers
{
    class SpawnHooks
    {
        public static void Apply()
        {
            On.WorldLoader.GeneratePopulation += WorldLoader_GeneratePopulation;
        }

        public static void UnApply()
        {
            On.WorldLoader.GeneratePopulation -= WorldLoader_GeneratePopulation;
        }

        static void WorldLoader_GeneratePopulation(On.WorldLoader.orig_GeneratePopulation orig, object self, bool fresh)
        {
            orig(self, fresh);

            OverseersPlugin.Logger_.LogInfo("infiltrated population generation!");

            if (self is WorldLoader wl)
            {
                int count = Random.Range(ConfigMenu.MinOverseers, ConfigMenu.MaxOverseers);
                for (int i = 0; i < count; i++)
                {
                    wl.world.offScreenDen.entitiesInDens.Add(new AbstractCreature(
                        wl.world,
                        StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.Overseer),
                        null,
                        new WorldCoordinate(wl.world.offScreenDen.index, 1, 1, 0),
                        wl.game.GetNewID()));
                }
                OverseersPlugin.Logger_.LogInfo($"added {count} overseers");
            }

        }

    }
}
