using CityResources;
using Infrastructure.Residents;
using Infrastructure.Tick;
using UnityEngine;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class House : Residential, Tickable
    {
        public float ElectricityDrain = 1;

        private Electricity _elec;

        public House()
        {
            Name = "House";
            BuildingPrefabPath = "Basic House";
        }

        public override void OnEntityProduced(GridSystem grid)
        {
            _elec = grid.ParentCity.GetResource<Electricity>();
        }

        public override void OnWorldGridTileCreated(WorldGridTile tile)
        {
            Material mat = tile.Model.GetComponent<MeshRenderer>().material;
            //Want to change the colour randomly.
            mat.color = Color.grey;
            //ITS GREY NOT GRAY.
        }

        public void Tick(float time)
        {
            if (IsVacant) return;
            float request = ElectricityDrain * time;
            float recieved = _elec.Recieve(request);
            if (recieved != request)
            {
                //If the amount we requested is not the same as what we got, track this.
                //This will be to handle uphappiness later.
            }
        }
    }
}
