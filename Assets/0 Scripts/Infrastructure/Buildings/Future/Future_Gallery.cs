
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Future_Gallery : HappinessShop
    {
        public Future_Gallery()
        {
            PrefabName = "Future/Gallery";
            Name = "Gallery";
            Cost = 950;

            AddJobs(4);
        }
    }
}
