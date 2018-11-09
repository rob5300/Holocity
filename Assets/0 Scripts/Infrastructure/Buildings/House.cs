using UnityEngine;

namespace Infrastructure.Grid.Entities.Buildings
{
    public class House : Building
    {
        public House()
        {
            Name = "House";
            BuildingPrefabPath = "Basic House";
        }

        public override void OnWorldGridTileCreated(WorldGridTile tile)
        {
            Material mat = tile.Model.GetComponent<MeshRenderer>().material;
            //Want to change the colour randomly.
        }
    }
}
