using Smod2;
using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;

namespace Smod.TestPlugin
{
    class EventHandler : IEventHandlerRoundStart, IEventHandlerWarheadDetonate//, IEventHandlerWarheadStopCountdown, IEventHandlerWarheadStartCountdown
    {
        private Plugin plugin;

        public EventHandler(Plugin plugin)
        {
            this.plugin = plugin;
        }

//        private System.Timers.Timer ulck = new System.Timers.Timer();
//        private void InitTimer(uint time)
//        {
//            ulck.Interval = time * 1000;
//            ulck.AutoReset = false;
//            ulck.Enabled = true;
//            ulck.Elapsed += delegate
//            {
//                foreach (Smod2.API.Door door in PluginManager.Manager.Server.Map.GetDoors())
//                {
//                    door.Locked = false;
//                }
//                ulck.Enabled = false;
//            };
//        }

        public void OnDetonate()
        {
//            ulck.Enabled = false;
            if (plugin.GetConfigBool("ss_nuke_destroy_doors"))
            {
                foreach (Smod2.API.Door door in PluginManager.Manager.Server.Map.GetDoors())
                {
                    if (door.Position.y <= 900)
                    {
                        door.Destroyed = true;
                    }
                }
            }
//            if (plugin.GetConfigBool("ss_nuke_destroy_items"))
//            {
//                foreach (int ind in System.Enum.GetValues(typeof(ItemType)))
//                {
//                    if (ind == -1) { continue; }
//                    foreach (Item item in PluginManager.Manager.Server.Map.GetItems(((ItemType)ind), false))
//                    {
//                        if (item.GetPosition().y <= 900)
//                        {
//                            item.Remove();
//                        }
//                    }
//                }
//            }
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            if (plugin.GetConfigBool("ss_doors_stay_shut"))
            {
                foreach (Smod2.API.Door door in ev.Server.Map.GetDoors())
                {
                    if (door.Name != "NUKE_SURFACE")
                    {
                        door.DontOpenOnWarhead = true;
                    }
                }
            }
        }

//        public void OnStartCountdown(WarheadStartEvent ev)
//        {
            //InitTimer(2);
//        }

//        public void OnStopCountdown(WarheadStopEvent ev)
//        {
//            ulck.Enabled = false;
//        }
//    }
}