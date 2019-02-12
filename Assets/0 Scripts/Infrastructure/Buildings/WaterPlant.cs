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
            base.OnEntityProduced(grid);

            //Get or make the water resource
            if (!ResourceReferenceManager.HaveResourceConnection<Electricity>())
            {
                _waterResource = new Water();
                ResourceReferenceManager.AddResource(_waterResource);
            }
            else
                _waterResource = ResourceReferenceManager.GetResource<Water>();
        }

        public void Tick(float time)
        {
            _waterResource.Add(PowerIncreaseRate * time);
        }
    }
}
