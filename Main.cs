using Smod2;
using Smod2.Attributes;
using System.Collections.Generic;
using UnityEngine.Networking;
using MEC;
using System.Linq;


namespace StayShut
{
	[PluginDetails(
		author = "storm37000",
		name = "StayShut",
		description = "Keeps the doors you want to be shut, shut (after a configurable time).",
		id = "s37k.stayshut",
		version = "1.0.11",
		SmodMajor = 3,
		SmodMinor = 2,
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
		}

		public bool UpToDate { get; private set; } = true;

		public void outdatedmsg()
		{
			this.Error("Your version is out of date, please update the plugin and restart your server when it is convenient for you.");
		}

		IEnumerator<float> UpdateChecker()
		{
			string[] hosts = { "https://storm37k.com/addons/", "http://74.91.115.126/addons/" };
			bool fail = true;
			string errorMSG = "";
			foreach (string host in hosts)
			{
				using (UnityWebRequest webRequest = UnityWebRequest.Get(host + this.Details.name + ".ver"))
				{
					// Request and wait for the desired page.
					yield return Timing.WaitUntilDone(webRequest.SendWebRequest());

					if (webRequest.isNetworkError || webRequest.isHttpError)
					{
						errorMSG = webRequest.error;
					}
					else
					{
						if (webRequest.downloadHandler.text != this.Details.version)
						{
							outdatedmsg();
							UpToDate = false;
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
			this.AddEventHandlers(new EventHandler(this));
			this.AddConfig(new Smod2.Config.ConfigSetting("ss_nuke_destroy_doors", false, Smod2.Config.SettingType.BOOL, true, ""));
//			this.AddConfig(new Smod2.Config.ConfigSetting("ss_nuke_destroy_items", true, Smod2.Config.SettingType.BOOL, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("ss_autoshut_time", 0, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("ss_autoshut_doors", new string[] { "GATE_A", "GATE_B" }, Smod2.Config.SettingType.LIST, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("ss_079_blacklist_doors", new string[] {}, Smod2.Config.SettingType.LIST, true, ""));
//			if (ConfigManager.Manager.Config.GetBoolValue("lock_gates_on_countdown", true))
//			{
//				this.Error("lock_gates_on_countdown MUST be set to false for this plugin to be able to work safely!");
//			}

			try
			{
				string file = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(Smod2.ConfigManager.Manager.Config.GetConfigPath()), "s37k_g_disableVcheck*", System.IO.SearchOption.TopDirectoryOnly).FirstOrDefault();
				if (file == null)
				{
					Timing.RunCoroutine(UpdateChecker());
				}
				else
				{
					this.Info("Version checker is disabled.");
				}
			}
			catch(System.Exception)
			{
				Timing.RunCoroutine(UpdateChecker());
			}

		}
	}
}
