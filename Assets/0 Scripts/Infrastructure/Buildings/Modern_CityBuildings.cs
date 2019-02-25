
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Modern_CityBuildings : Building
    {
        public Modern_CityBuildings()
        {
            Name = "City Buildings";
            PrefabName = "City Buildings Modern Day";
            Cost = 200;
            category = BuildingCategory.Commercial;
        }
    }
}
