
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Modern_Office : Commercial2x2
    {
        public Modern_Office()
        {
            PrefabName = "Modern/Office";
            Name = "Offices";
            Cost = 1500;

            AddJobs(25);
        }
    }
}
