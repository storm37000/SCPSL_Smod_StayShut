using Smod2.Events;
using Smod2.EventHandlers;
using System.Collections.Generic;
using MEC;

namespace StayShut
{
	class EventHandler : IEventHandlerWarheadDetonate, IEventHandlerDoorAccess, IEventHandlerWaitingForPlayers, IEventHandler079Door, IEventHandler079Lock//, IEventHandlerWarheadStopCountdown, IEventHandlerWarheadStartCountdown, IEventHandlerRoundStart
	{
		private Main plugin;
		private int autoshuttime;
		string[] doorlist;
		string[] scp079blklst;

		public EventHandler(Main plugin)
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

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			autoshuttime = plugin.GetConfigInt("ss_autoshut_time");
			doorlist = plugin.GetConfigList("ss_autoshut_doors");
			scp079blklst = plugin.GetConfigList("ss_079_blacklist_doors");
		}

		public void OnDetonate()
		{
//            ulck.Enabled = false;
			if (plugin.GetConfigBool("ss_nuke_destroy_doors"))
			{
				foreach (Smod2.API.Door door in Smod2.PluginManager.Manager.Server.Map.GetDoors())
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

		IEnumerator<float> doorAutoShut(Smod2.API.Door door)
		{
			plugin.Debug("attempting to close door: " + door.Name + " in " + autoshuttime + " seconds");
			yield return Timing.WaitForSeconds(autoshuttime);
			if (door.Destroyed == false && door.Locked == false && door.Open == true)
			{
				door.Open = false;
				plugin.Debug("Closing door: " + door.Name);
			}
		}

		public void OnDoorAccess(PlayerDoorAccessEvent ev)
		{
			if (ev.Door.Open == false && autoshuttime != 0 && System.Array.IndexOf(doorlist, ev.Door.Name) !=-1)
			{
				Timing.RunCoroutine(doorAutoShut(ev.Door), Segment.FixedUpdate);
			}
		}

		public void On079Door(Player079DoorEvent ev)
		{
			if (ev.Door.Open == false && System.Array.IndexOf(scp079blklst, ev.Door.Name) != -1)
			{
				ev.Allow = false;
				ev.APDrain = 0;
			}
		}

		public void On079Lock(Player079LockEvent ev)
		{
			if (System.Array.IndexOf(scp079blklst, ev.Door.Name) != -1)
			{
				ev.Allow = false;
				ev.APDrain = 0;
			}
		}

		//		public void OnRoundStart(RoundStartEvent ev)
		//        {
		//            if (plugin.GetConfigBool("ss_doors_stay_shut"))
		//            {
		//                if (!ConfigManager.Manager.Config.GetBoolValue("lock_gates_on_countdown", true))
		//                {
		//                    foreach (Smod2.API.Door door in ev.Server.Map.GetDoors())
		//                    {
		//                        if (door.Name != "NUKE_SURFACE")
		//                        {
		//                            door.DontOpenOnWarhead = true;
		//                        }
		//                    }
		//                } else
		//                {
		//					safe = false;
		//                    plugin.Error("'lock_gates_on_countdown: false' MUST be in your config for this plugin to be able to work safely!");
		//                }
		//            }
		//        }

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