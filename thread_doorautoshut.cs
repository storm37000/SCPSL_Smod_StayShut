using System.Threading;

namespace StayShut
{
    class thread_doorautoshut
    {
        public thread_doorautoshut(Smod2.Plugin plugin, Smod2.API.Door door, int time)
        {
			plugin.Debug("attempting to close door: " + door.Name + " in " + time/1000 + " seconds");
            Thread.Sleep(time);
			if (door.Destroyed == false && door.Locked == false && door.Open == true)
			{
				door.Open = false;
				plugin.Debug("Closing door: " + door.Name);
			}
        }
    }
}