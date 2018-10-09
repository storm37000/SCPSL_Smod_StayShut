﻿using Smod2;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Threading;

namespace Smod.TestPlugin
{
    class EventHandler : IEventHandlerRoundStart, IEventHandlerWarheadDetonate, IEventHandlerDoorAccess//, IEventHandlerWarheadStopCountdown, IEventHandlerWarheadStartCountdown
	{
        private Plugin plugin;
		private bool safe = true;

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

		public void OnDoorAccess(PlayerDoorAccessEvent ev)
		{
			if (safe && ev.Door.Open == false && plugin.GetConfigInt("ss_autoshut_time") != 0)
			{
				if (System.Array.IndexOf(plugin.GetConfigList("ss_autoshut_doors"),ev.Door.Name) !=-1)
				{
					Thread doorautoshutthread = new Thread(new ThreadStart(() => new doorautoshutthread(this.plugin, ev.Door, plugin.GetConfigInt("ss_autoshut_time"))));
					doorautoshutthread.Start();
				}
			}
		}

		public void OnRoundStart(RoundStartEvent ev)
        {
            if (plugin.GetConfigBool("ss_doors_stay_shut"))
            {
                if (!ConfigManager.Manager.Config.GetBoolValue("lock_gates_on_countdown", true))
                {
                    foreach (Smod2.API.Door door in ev.Server.Map.GetDoors())
                    {
                        if (door.Name != "NUKE_SURFACE")
                        {
                            door.DontOpenOnWarhead = true;
                        }
                    }
                } else
                {
					safe = false;
                    plugin.Error("lock_gates_on_countdown MUST be set to false for this plugin to be able to work safely!");
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
    }
}