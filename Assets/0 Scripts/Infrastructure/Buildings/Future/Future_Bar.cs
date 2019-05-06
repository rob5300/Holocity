
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Future_Bar : Commercial
    {
        public Future_Bar()
        {
            PrefabName = "Future/BarUV";
            Name = "Bar";
            Cost = 300;

            AddJobs(5);
        }
    }
}
