
namespace Infrastructure.Grid.Entities.Buildings
{
    public class Flat2 : House
    {
        public Flat2()
        {
            Name = "Small Flats";
            PrefabName = "Future/Flats2F";
            Cost = 3000;
            category = BuildingCategory.Residential;
        }
    }
}
