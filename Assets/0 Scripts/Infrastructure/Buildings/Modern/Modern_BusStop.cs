
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Modern_BusStop : HappinessShop
    {
        public Modern_BusStop()
        {
            PrefabName = "Modern/Bus Stop";
            Name = "Bus Stop";
            Cost = 700;

            AddJobs(2);
        }
    }
}
