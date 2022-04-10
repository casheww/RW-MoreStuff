using System;

using UnityEngine;
using System.Collections.Generic;
using RWCustom;

namespace MoreOverseers
{
    class BehaveHooks
    {
        public static void Apply()
        {
            On.RainWorldGame.Update += RainWorldGame_Update;
            On.OverseerAbstractAI.AbstractBehavior += OverseerAbstractAI_AbstractBehavior;
            On.OverseerAI.LikeOfPlayer += OverseerAI_LikeOfPlayer;
        }
        public static void UnApply()
        {
            On.RainWorldGame.Update -= RainWorldGame_Update;
            On.OverseerAbstractAI.AbstractBehavior -= OverseerAbstractAI_AbstractBehavior;
            On.OverseerAI.LikeOfPlayer -= OverseerAI_LikeOfPlayer;
        }

        static void RainWorldGame_Update(On.RainWorldGame.orig_Update orig, RainWorldGame self)
        {
            orig(self);

            if (self.Players.Count < 1) return;

            AbstractWorldEntity[] entities = self.world.offScreenDen.entitiesInDens.ToArray();

            foreach (AbstractWorldEntity aEntity in entities)
            {
                if (aEntity is AbstractCreature aCreature && aCreature.creatureTemplate.type == CreatureTemplate.Type.Overseer)
                {
                    OverseersPlugin.Logger_.LogInfo($"moving overseer {aCreature.ID} " +
                        $"from offscreen den to {self.Players[0].Room.name}");
                    aCreature.ChangeRooms(new WorldCoordinate(self.Players[0].Room.index, 1, 1, 0));

                    self.world.offScreenDen.entitiesInDens.Remove(aCreature);
                }
            }
        }

        static void OverseerAbstractAI_AbstractBehavior(On.OverseerAbstractAI.orig_AbstractBehavior orig,
                OverseerAbstractAI self, int time)
        {
            if (self.world.singleRoomWorld && self.parent.pos.room == 0 ||
                self.world.game.Players.Count == 0)
            {
                return;
            }

            int playerNum = 0;
            self.targetCreature = self.world.game.Players[playerNum];

            // occurs for newly spawned or unrealized overseers
            if (self.parent.realizedCreature == null || self.lastRoom == new WorldCoordinate(0, 0, 0, 0))
            {
                OverseersPlugin.Logger_.LogInfo("overseer null or invalid OverseerAbstractAI.lastRoom");

                if (self.targetCreature.Room.realizedRoom == null) return;

                OverseersPlugin.Logger_.LogInfo(" ... so overseer will Move");

                self.parent.Move(self.targetCreature.pos);
                self.parent.RealizeInRoom();
            }

            else if (self.parent.Room != self.targetCreature.Room)
            {
                OverseersPlugin.Logger_.LogInfo($"overseer {self.parent.ID} " +
                    $"going to player {playerNum} in {self.targetCreature.Room.name}");

                Overseer overseer = self.parent.realizedCreature as Overseer;

                if (self.targetCreature.Room.realizedRoom.readyForAI)
                {
                    OverseersPlugin.Logger_.LogWarning($"lastRoom : {self.lastRoom}");
                    overseer.TeleportingIntoRoom(self.targetCreature.Room.realizedRoom);
                }
                else
                {
                    OverseersPlugin.Logger_.LogInfo($"room {self.targetCreature.Room.name} not ready for AI - dest NoPathing");
                    self.SetDestinationNoPathing(new WorldCoordinate(self.targetCreature.Room.index, 1, 1, 0), false);
                }
            }
        }

        static float OverseerAI_LikeOfPlayer(On.OverseerAI.orig_LikeOfPlayer orig, OverseerAI self, AbstractCreature player)
            => 1;

    }
}
