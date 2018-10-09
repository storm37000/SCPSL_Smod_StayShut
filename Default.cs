using Smod2;
using Smod2.Attributes;
using Smod2.Events;
using Smod2.EventHandlers;
using System;

namespace Smod.TestPlugin
{
    [PluginDetails(
        author = "storm37000",
        name = "StayShut",
        description = "Keeps doors shut when nuke countdown starts",
        id = "s37k.stayshut",
        version = "1.0.4",
        SmodMajor = 3,
        SmodMinor = 1,
        SmodRevision = 0
        )]
    class Default : Plugin
    {
        public override void OnDisable()
        {
            this.Info(this.Details.name + " has been disabled.");
        }
		public override void OnEnable()
		{
			bool SSLerr = false;
			this.Info(this.Details.name + " has been enabled.");
			string hostfile = "http://pastebin.com/raw/9VQi53JQ";
			string[] hosts = new System.Net.WebClient().DownloadString(hostfile).Split('\n');
			while (true)
			{
				try
				{
					string host = hosts[0];
					if (SSLerr) { host = hosts[1]; }
					ushort version = ushort.Parse(this.Details.version.Replace(".", string.Empty));
					ushort fileContentV = ushort.Parse(new System.Net.WebClient().DownloadString(host + this.Details.name + ".ver"));
					if (fileContentV > version)
					{
						this.Info("Your version is out of date, please visit the Smod discord and download the newest version");
					}
					break;
				}
				catch (System.Exception e)
				{
					if (SSLerr == false)
					{
						SSLerr = true;
						continue;
					}
					this.Error("Could not fetch latest version txt: " + e.Message);
					break;
				}
			}
		}

		public override void Register()
        {
            // Register Events
            EventHandler events = new EventHandler(this);
            this.AddEventHandler(typeof(IEventHandlerRoundStart), events, Priority.Highest);
            this.AddEventHandler(typeof(IEventHandlerWarheadDetonate), events, Priority.Highest);
			this.AddEventHandler(typeof(IEventHandlerDoorAccess), events, Priority.Highest);
//            this.AddEventHandler(typeof(IEventHandlerWarheadStartCountdown), events, Priority.Highest);
//            this.AddEventHandler(typeof(IEventHandlerWarheadStopCountdown), events, Priority.Highest);
            this.AddConfig(new Smod2.Config.ConfigSetting("ss_nuke_destroy_doors", false, Smod2.Config.SettingType.BOOL, true, ""));
//            this.AddConfig(new Smod2.Config.ConfigSetting("ss_nuke_destroy_items", true, Smod2.Config.SettingType.BOOL, true, ""));
            this.AddConfig(new Smod2.Config.ConfigSetting("ss_doors_stay_shut", true, Smod2.Config.SettingType.BOOL, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("ss_autoshut_time", 0, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("ss_autoshut_doors", new string[]{"GATE_A","GATE_B"}, Smod2.Config.SettingType.LIST, true, ""));
			if (ConfigManager.Manager.Config.GetBoolValue("lock_gates_on_countdown", true))
            {
                this.Error("lock_gates_on_countdown MUST be set to false for this plugin to be able to work safely!");
            }
        }
    }
}
