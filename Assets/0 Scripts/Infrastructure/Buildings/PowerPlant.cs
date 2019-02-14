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
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            
        }

        public void Tick(float time)
        {
            elecResource.Add(PowerIncreaseRate * time);
        }
    }
}
