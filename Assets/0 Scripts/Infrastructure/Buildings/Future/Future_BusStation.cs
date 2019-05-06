
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Future_BusStation : Commercial
    {
        public Future_BusStation()
        {
            PrefabName = "Future/BusstationUV";
            Name = "Bus Station";
            Cost = 500;

            AddJobs(3);
        }
    }
}
