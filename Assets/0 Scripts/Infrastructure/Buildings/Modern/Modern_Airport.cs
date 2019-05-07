
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Modern_Airport : Commercial
    {
        public Modern_Airport()
        {
            PrefabName = "Modern/Airport";
            Name = "Airport";
            Cost = 500;

            AddJobs(5);
        }
    }
}
