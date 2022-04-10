using BepInEx;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace MoreMice;

[BepInPlugin("casheww.more_mice", "MoreMice", "0.1.0")]
public class MoreMicePlugin : BaseUnityPlugin
{
    private void OnEnable()
    {
        On.WorldLoader.GeneratePopulation += WorldLoader_GeneratePopulation;
        On.LanternMouse.GenerateIVars += LanternMouse_GenerateIVars;
        On.StaticWorld.EstablishRelationship += StaticWorld_EstablishRelationship;
    }

    private void OnDisable()
    {
        On.WorldLoader.GeneratePopulation -= WorldLoader_GeneratePopulation;
        On.LanternMouse.GenerateIVars -= LanternMouse_GenerateIVars;
        On.StaticWorld.EstablishRelationship -= StaticWorld_EstablishRelationship;
    }

    private void WorldLoader_GeneratePopulation(On.WorldLoader.orig_GeneratePopulation orig, object self, bool fresh)
    {
        orig(self, fresh);

        if (self is not WorldLoader wl)
            return;

        foreach (World.CreatureSpawner spawner in wl.spawners)
        {
            AbstractRoom abstractRoom = wl.world.GetAbstractRoom(spawner.den);
            
            for (int i = 0; i < 16; i++)
            {
                if (UnityEngine.Random.value < 0.125f)
                    break;
                
                abstractRoom.MoveEntityToDen(new AbstractCreature(
                    wl.world,
                    StaticWorld.GetCreatureTemplate(CreatureTemplate.Type.LanternMouse),
                    null,
                    spawner.den,
                    wl.world.game.GetNewID()));
            }
        }
    }
    
    private void LanternMouse_GenerateIVars(On.LanternMouse.orig_GenerateIVars orig, LanternMouse self)
    {
        if (UnityEngine.Random.value < 0.5f)
        {
            HSLColor color = new HSLColor(UnityEngine.Random.value, 1f, 0.6f);
            self.iVars = new LanternMouse.IndividualVariations(UnityEngine.Random.value, color);
        }
        else
            orig(self);
    }
    
    private void StaticWorld_EstablishRelationship(On.StaticWorld.orig_EstablishRelationship orig,
        CreatureTemplate.Type a, CreatureTemplate.Type b, CreatureTemplate.Relationship relationship)
    {
        if (a == CreatureTemplate.Type.LanternMouse && b == CreatureTemplate.Type.Slugcat)
            orig(a, b, new CreatureTemplate.Relationship(CreatureTemplate.Relationship.Type.PlaysWith, 0.4f));
        else
            orig(a, b, relationship);
    }
    
}
