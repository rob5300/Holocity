using UnityEngine;

namespace Infrastructure.Grid.Entities
{
    public class Road : TileEntity
    {
        private static bool _cachedPrefabs = false;
        private static GameObject[] RoadPrefabs;
        public static string ResourcesFolderpath = "Roads/";

        public bool CarryingPower = false;
        public bool CarryingWater = false;

        private float _zRotationToApply = 0;

        public override GameObject GetModel()
        {
            if (!_cachedPrefabs)
            {
                LoadRoadPrefabs();
            }
            //We need to know if we have roads in 4 tile positions around this tile.
            //Example: We are #, tiles we want are N S E W.
            //   5 - - - - -
            //   4 - - N - -
            //   3 - W # E -
            //   2 - - S - -
            // y 1 - - - - -
            //   x 0 1 2 3 4
            //If a tile doesnt exist, wrong type or is out of bounds, it counts as a false reading.

            //Get if we have roads or not in the 4 tile positions;
            RoadDirection roadD = new RoadDirection();
            roadD.N = CheckTileForRoad(new Vector2Int(0, 1));
            roadD.E = CheckTileForRoad(new Vector2Int(1, 0));
            roadD.S = CheckTileForRoad(new Vector2Int(0, -1));
            roadD.W = CheckTileForRoad(new Vector2Int(-1, 0));

            //May need optimising


            return null;
            
        }

        private bool CheckTileForRoad(Vector2Int tilePosition)
        {
            Vector2Int positionToCheck = ParentTile.Position + tilePosition;
            GridTile tile = ParentTile.ParentGridSystem.GetTile(positionToCheck);
            return tile.Entity is Road;
        }

        private static void LoadRoadPrefabs()
        {
            //Cache and load all road prefabs.
            RoadPrefabs = new GameObject[4];
            RoadPrefabs[0] = Resources.Load<GameObject>(ResourcesFolderpath + "Road EW");
            RoadPrefabs[1] = Resources.Load<GameObject>(ResourcesFolderpath + "Road NSEW");
            RoadPrefabs[2] = Resources.Load<GameObject>(ResourcesFolderpath + "Road NSW");
            RoadPrefabs[3] = Resources.Load<GameObject>(ResourcesFolderpath + "Road NW");
        }
    }
}

public struct RoadDirection
{
    public bool N;
    public bool E;
    public bool S;
    public bool W;
}
