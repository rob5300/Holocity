
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Future_Farm : Commercial
    {
        public Future_Farm()
        {
            PrefabName = "Future/FarmUV";
            Name = "Farm";
            Cost = 800;

            AddJobs(3);
        }
    }
}
