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
            elecResource = new Electricity();
            AddNewResource(typeof(Electricity), new ResourceData(elecResource, this));
        }

        public override void Tick(float time)
        {
            base.Tick(time);

            elecResource.Add(PowerIncreaseRate * time);
        }
    }
}
