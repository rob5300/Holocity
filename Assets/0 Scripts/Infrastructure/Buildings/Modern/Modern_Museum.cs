
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Modern_Museum : HappinessShop
    {
        public Modern_Museum()
        {
            PrefabName = "Modern/Museum";
            Name = "Museum";
            Cost = 1000;

            AddJobs(5);
        }
    }
}
