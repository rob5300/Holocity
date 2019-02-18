using CityResources;
using Infrastructure.Tick;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class PowerPlant : Building, Tickable
    {
        public int PowerIncreaseRate = 5;

        private Electricity elecResource;
        private ResourceData elecData;

        public PowerPlant()
        {
            PrefabName = "Powerplant";
            Cost = 25000;
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            elecResource = new Electricity(ParentTile.ParentGridSystem.Id);
            elecData = new ResourceData(elecResource, this);
            elecData.id = 555;
            AddNewResource(typeof(Electricity), elecData);

            //We need to call base to distribute our resource AFTER we make ours.
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
