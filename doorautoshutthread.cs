using System.Threading;

namespace Smod.TestPlugin
{
    class doorautoshutthread
    {
        public doorautoshutthread(Smod2.Plugin plugin, Smod2.API.Door door, int time)
        {
            Thread.Sleep(time);
			if (door.Destroyed == false && door.Locked == false && door.Open == true)
			{
				door.Open = false;
			}
        }
    }
}