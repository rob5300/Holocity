
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Future_Airport : Commercial
    {
        public Future_Airport()
        {
            PrefabName = "Future/Airport";
            Name = "Airport";
            Cost = 1500;

            AddJobs(10);
        }
    }
}
