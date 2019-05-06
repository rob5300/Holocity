
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Future_Bank : Commercial
    {
        public Future_Bank()
        {
            PrefabName = "Future/BankUV";
            Name = "Bank";
            Cost = 400;

            AddJobs(10);
        }
    }
}
