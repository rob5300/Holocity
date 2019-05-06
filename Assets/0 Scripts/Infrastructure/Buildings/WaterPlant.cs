using CityResources;
using Infrastructure.Tick;
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class WaterPlant : Commercial, ITickable
    {
        public int PowerIncreaseRate = 15;

        private Water _waterResource;
        private ResourceData waterData;

        public WaterPlant()
        {
            Name = "Water Plant";
            PrefabName = "Waterplant";
            Cost = 2500;
            category = BuildingCategory.Resource;

            Jobs = new List<Job>();
            for (int i = 0; i < 5; i++)
            {
                Jobs.Add(new Job(this));
            }
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
