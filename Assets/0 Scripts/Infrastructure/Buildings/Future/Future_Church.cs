
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Future_Church : Commercial
    {
        public Future_Church()
        {
            PrefabName = "Future/Church";
            Name = "Church";
            Cost = 300;

            AddJobs(2);
        }
    }
}
