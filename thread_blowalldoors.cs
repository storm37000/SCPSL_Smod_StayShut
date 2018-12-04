
namespace StayShut
{
    class thread_blowalldoors
	{
        public thread_blowalldoors()
        {
			foreach (Smod2.API.Door door in Smod2.PluginManager.Manager.Server.Map.GetDoors())
			{
				if (door.Position.y <= 900)
				{
					door.Destroyed = true;
				}
			}
		}
    }
}