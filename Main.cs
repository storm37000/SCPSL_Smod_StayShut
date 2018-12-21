using Smod2;
using Smod2.Attributes;
using Smod2.Events;
using Smod2.EventHandlers;

namespace StayShut
{
    [PluginDetails(
        author = "storm37000",
        name = "StayShut",
        description = "Keeps doors shut when nuke countdown starts",
        id = "s37k.stayshut",
        version = "1.0.7",
        SmodMajor = 3,
        SmodMinor = 1,
        SmodRevision = 0
        )]
    class Main : Plugin
    {
        public override void OnDisable()
        {
            this.Info(this.Details.name + " has been disabled.");
        }
		public override void OnEnable()
		{
			this.Info(this.Details.name + " has been enabled.");
			string[] hosts = { "https://storm37k.com/addons/", "http://74.91.115.126/addons/" };
			ushort version = ushort.Parse(this.Details.version.Replace(".", string.Empty));
			bool fail = true;
			string errorMSG = "";
			foreach (string host in hosts)
			{
				using (UnityEngine.WWW req = new UnityEngine.WWW(host + this.Details.name + ".ver"))
				{
					while (!req.isDone) { }
					errorMSG = req.error;
					if (string.IsNullOrEmpty(req.error))
					{
						ushort fileContentV = 0;
						if (!ushort.TryParse(req.text, out fileContentV))
						{
							errorMSG = "Parse Failure";
							continue;
						}
						if (fileContentV > version)
						{
							this.Error("Your version is out of date, please visit the Smod discord and download the newest version");
						}
						fail = false;
						break;
					}
				}
			}
			if (fail)
			{
				this.Error("Could not fetch latest version txt: " + errorMSG);
			}
		}

		public override void Register()
        {
            // Register Events
            EventHandler events = new EventHandler(this);
            this.AddEventHandler(typeof(IEventHandlerRoundStart), events, Priority.Highest);
            this.AddEventHandler(typeof(IEventHandlerWarheadDetonate), events, Priority.Highest);
			this.AddEventHandler(typeof(IEventHandlerDoorAccess), events, Priority.Highest);
//			this.AddEventHandler(typeof(IEventHandlerWarheadStartCountdown), events, Priority.Highest);
//			this.AddEventHandler(typeof(IEventHandlerWarheadStopCountdown), events, Priority.Highest);
//			this.AddConfig(new Smod2.Config.ConfigSetting("ss_doors_stay_shut", true, Smod2.Config.SettingType.BOOL, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("ss_nuke_destroy_doors", false, Smod2.Config.SettingType.BOOL, true, ""));
//			this.AddConfig(new Smod2.Config.ConfigSetting("ss_nuke_destroy_items", true, Smod2.Config.SettingType.BOOL, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("ss_autoshut_time", 0, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("ss_autoshut_doors", new string[] { "GATE_A", "GATE_B" }, Smod2.Config.SettingType.LIST, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("ss_079_blacklist_doors", new string[] {}, Smod2.Config.SettingType.LIST, true, ""));
//			if (ConfigManager.Manager.Config.GetBoolValue("lock_gates_on_countdown", true))
//			{
//				this.Error("lock_gates_on_countdown MUST be set to false for this plugin to be able to work safely!");
//			}
		}
    }
}
