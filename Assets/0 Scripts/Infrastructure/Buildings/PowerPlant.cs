using CityResources;
using Infrastructure.Tick;
using System.Collections.Generic;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class PowerPlant : Commercial, ITickable
    {
        public int PowerIncreaseRate = 5;

        private Electricity elecResource;
        private ResourceData elecData;

        public PowerPlant()
        {
            Name = "Powerplant";
            PrefabName = "Powerplant Future";
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
            elecResource = new Electricity(ParentTile.ParentGridSystem.Id);
            elecData = new ResourceData(elecResource, this);
            elecData.id = 555;
            AddNewResource(typeof(Electricity), elecData);

            //Must be called after soo the resources exist.
            base.OnEntityProduced(grid);
        }

        public override void Tick(float time)
        {
            base.Tick(time);

            elecResource.Add(PowerIncreaseRate * time);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            elecData.Destroy();
        }
    }
}
