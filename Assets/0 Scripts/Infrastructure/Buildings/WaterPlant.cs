using CityResources;
using Infrastructure.Tick;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class WaterPlant : ResourceConductEntity, Tickable
    {
        public int PowerIncreaseRate = 15;

        private Water _waterResource;

        public WaterPlant()
        {
            PrefabName = "Waterplant";
            Cost = 10000;
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            
        }

        public void Tick(float time)
        {
            _waterResource.Add(PowerIncreaseRate * time);
        }
    }
}
