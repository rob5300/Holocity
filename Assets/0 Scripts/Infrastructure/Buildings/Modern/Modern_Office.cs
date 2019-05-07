
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Modern_Office : Commercial
    {
        public Modern_Office()
        {
            PrefabName = "Modern/Office Building";
            Name = "Offices";
            Cost = 1500;

            AddJobs(25);
        }
    }
}
