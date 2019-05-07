
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Modern_Powerplant : PowerPlant
    {
        public Modern_Powerplant()
        {
            PrefabName = "Modern/Powerplant";
            Name = "Power Plant";
            Cost = 500;

            AddJobs(5);
        }
    }
}
