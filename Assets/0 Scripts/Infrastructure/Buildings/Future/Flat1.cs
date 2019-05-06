
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Flat1 : House
    {
        public Flat1()
        {
            Name = "Flat Highrise";
            PrefabName = "Future/Flats1F";
            Cost = 3000;
            category = BuildingCategory.Residential;
        }
    }
}
