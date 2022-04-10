using System;

namespace MoreOverseers.Interactions
{
    class ConversationHooks
    {
        public static void Apply()
        {
            On.SLOracleBehaviorHasMark.MoonConversation.AddEvents += MoonConversation_AddEvents;
        }
        public static void UnApply()
        {
            On.SLOracleBehaviorHasMark.MoonConversation.AddEvents -= MoonConversation_AddEvents;
        }

        static void MoonConversation_AddEvents(On.SLOracleBehaviorHasMark.MoonConversation.orig_AddEvents orig,
                SLOracleBehaviorHasMark.MoonConversation self)
        {
            orig(self);

            if (self.id == Conversation.ID.MoonFirstPostMarkConversation
                && self.State.neuronsLeft >= 4)
            {
                switch (ConfigMenu.ColourMode_)
                {
                    case ConfigMenu.ColourMode.RandomStatic:
                        self.events.Add(new Conversation.TextEvent(self, 5,
                            self.Translate("Quite the collection of friends you have here, <PlayerName>!"), 0));
                        self.events.Add(new Conversation.TextEvent(self, 0,
                            self.Translate("My friends have been keeping close watch on things of late...<LINE>" +
                            "Hello, my friends..."), 10));
                        break;

                    case ConfigMenu.ColourMode.RandomStrobe:
                        self.events.Add(new Conversation.TextEvent(self, 5,
                            self.Translate("Oh, and quite the party you have brought with you!"), 0));
                        self.events.Add(new Conversation.TextEvent(self, 0,
                            self.Translate("How fashionable! It is so good to see this many colours after so long..."), 0));
                        break;

                    case ConfigMenu.ColourMode.AllMoon:
                        self.events.Add(new Conversation.TextEvent(self, 5,
                            self.Translate("You've been looking after my overseers, <PlayerName>...<LINE>" +
                            "Thank you."), 0));
                        break;

                    case ConfigMenu.ColourMode.Default:
                    case ConfigMenu.ColourMode.AllPebbles:
                        self.events.Add(new Conversation.TextEvent(self, 5,
                            self.Translate("So many overseers from Five Pebbles..."), 0));
                        self.events.Add(new Conversation.TextEvent(self, 0,
                            self.Translate("I do wonder if he still cares about what goes on outside his can."), 10));
                        break;

                    case ConfigMenu.ColourMode.AllGreenOverseerFromUnknownIterator:
                        self.events.Add(new Conversation.TextEvent(self, 5,
                            self.Translate("Someone is watching over you, <PlayerName>.<LINE>" +
                            "These overseers have travelled far..."), 0));
                        break;
                }

            }
        }

    }
}
