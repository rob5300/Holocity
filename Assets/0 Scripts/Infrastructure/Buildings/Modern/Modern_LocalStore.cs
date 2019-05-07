
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Modern_LocalStore : Commercial
    {
        public Modern_LocalStore()
        {
            PrefabName = "Modern/Local Store";
            Name = "Store";
            Cost = 750;

            AddJobs(5);
        }
    }
}
