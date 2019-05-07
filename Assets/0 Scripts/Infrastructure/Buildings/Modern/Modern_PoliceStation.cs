
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Modern_PoliceStation : HappinessShop
    {
        public Modern_PoliceStation()
        {
            PrefabName = "Modern/Police Station";
            Name = "Police Station";
            Cost = 500;

            AddJobs(5);
        }
    }
}
