
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Shop : Building
    {
        public Shop()
        {
            Name = "Shop";
            PrefabName = "Shop Fixed";
            Cost = 500;
            category = BuildingCategory.Commercial;
        }
    }
}
