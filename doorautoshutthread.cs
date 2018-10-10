using System.Threading;

namespace Smod.TestPlugin
{
    class doorautoshutthread
    {
        public doorautoshutthread(Smod2.Plugin plugin, Smod2.API.Door door, int time)
        {
			plugin.Info("attempting to close door: " + door.Name + " in " + time/1000 + " seconds");
            Thread.Sleep(time);
			if (door.Destroyed == false && door.Locked == false && door.Open == true)
			{
                plugin.Info("Closing door!");
                foreach (Door d in UnityEngine.Object.FindObjectsOfType<Door>())
                {
                    if (d.DoorName == door.Name && d.isOpen)
                    {
                        d.ChangeState(false);
                        break;
                    }
                }
			}
        }
    }
}