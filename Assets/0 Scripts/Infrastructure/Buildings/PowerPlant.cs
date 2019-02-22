using CityResources;
using Infrastructure.Tick;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class PowerPlant : Building, Tickable
    {
        public int PowerIncreaseRate = 5;

        private Electricity elecResource;

        public PowerPlant()
        {
            PrefabName = "Powerplant";
            Cost = 25000;
            category = BuildingCategory.Resource;
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            elecResource = grid.ParentCity.GetResource<Electricity>();
        }

        public void Tick(float time)
        {
            elecResource.Add(PowerIncreaseRate * time);
        }
    }
}
