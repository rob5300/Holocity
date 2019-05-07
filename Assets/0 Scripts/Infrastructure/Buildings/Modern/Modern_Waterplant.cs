
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Modern_Waterplant : WaterPlant
    {
        public Modern_Waterplant()
        {
            PrefabName = "Modern/Powerplant";
            Name = "Water Plant";
            Cost = 500;

            AddJobs(5);
        }
    }
}
