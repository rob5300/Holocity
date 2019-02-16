using CityResources;
using Infrastructure.Tick;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class WaterPlant : Building, Tickable
    {
        public int PowerIncreaseRate = 15;

        private Water _waterResource;

        public WaterPlant()
        {
            PrefabName = "Waterplant";
            Cost = 10000;
            _waterResource = new Water();
            AddNewResource(typeof(Water), new ResourceData(_waterResource, this));
        }

        public override void Tick(float time)
        {
            base.Tick(time);
            _waterResource.Add(PowerIncreaseRate * time);
        }
    }
}
