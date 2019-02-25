using CityResources;
using Infrastructure.Tick;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class WaterPlant : Building, Tickable
    {
        public int PowerIncreaseRate = 15;

        private Water _waterResource;
        private ResourceData waterData;

        public WaterPlant()
        {
            Name = "Water Plant";
            PrefabName = "Waterplant";
            Cost = 10000;
            category = BuildingCategory.Resource;
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            _waterResource = new Water(ParentTile.ParentGridSystem.Id);
            waterData = new ResourceData(_waterResource, this);
            AddNewResource(typeof(Water), waterData);

            //We need to call base to distribute our resource AFTER we make ours.
            base.OnEntityProduced(grid);
        }

        public override void Tick(float time)
        {
            base.Tick(time);
            _waterResource.Add(PowerIncreaseRate * time);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            waterData.Destroy();
        }
    }
}
