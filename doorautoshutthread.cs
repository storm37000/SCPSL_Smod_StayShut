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
				door.Open = false;
				plugin.Info("Closing door!");
			}
        }
    }
}