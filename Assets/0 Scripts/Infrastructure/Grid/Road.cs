using UnityEngine;

namespace Infrastructure.Grid.Entities
{
    public class Road : ResourceConductEntity
    {
        private static bool _cachedPrefabs = false;
        private static GameObject[] RoadPrefabs;
        public static string RoadPath = "Roads/";

        public bool CarryingPower = false;
        public bool CarryingWater = false;

        private float _yRotationToApply = 0;
        private WorldGridTile worldTile;

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

            int numTrue = 0;
            if (roadD.N) numTrue++;
            if (roadD.E) numTrue++;
            if (roadD.S) numTrue++;
            if (roadD.W) numTrue++;

            //Check for road positions and decide what model to use and how to transform it (if at all).
            //We currently have to check for all 15 possible variations. Can be made more efficient but wont be very readable.

            if (numTrue == 1)
            {
                //One road piece
                if (roadD.N && !roadD.E && !roadD.S && !roadD.W)
                {
                    _yRotationToApply = 90;
                    return RoadPrefabs[0];
                }
                if (!roadD.N && roadD.E && !roadD.S && !roadD.W)
                {
                    return RoadPrefabs[0];
                }
                if (!roadD.N && !roadD.E && roadD.S && !roadD.W)
                {
                    _yRotationToApply = 90;
                    return RoadPrefabs[0];
                }
                if (!roadD.N && !roadD.E && !roadD.S && roadD.W)
                {
                    return RoadPrefabs[0];
                }
            }
            else if (numTrue == 2)
            {
                //Two road pieces
                if (roadD.N && !roadD.E && roadD.S && !roadD.W)
                {
                    _yRotationToApply = 90;
                    return RoadPrefabs[0];
                }
                if (!roadD.N && roadD.E && !roadD.S && roadD.W)
                {
                    return RoadPrefabs[0];
                }
                if (roadD.N && roadD.E && !roadD.S && !roadD.W)
                {
                    _yRotationToApply = 90;
                    return RoadPrefabs[3];
                }
                if (!roadD.N && roadD.E && roadD.S && !roadD.W)
                {
                    _yRotationToApply = 180;
                    return RoadPrefabs[3];
                }
                if (!roadD.N && !roadD.E && roadD.S && roadD.W)
                {
                    _yRotationToApply = -90;
                    return RoadPrefabs[3];
                }
                if (roadD.N && !roadD.E && !roadD.S && roadD.W)
                {
                    return RoadPrefabs[3];
                }
            }
            else if (numTrue == 3)
            {
                //Three road pieces
                if (!roadD.N && roadD.E && roadD.S && roadD.W)
                {
                    _yRotationToApply = -90;
                    return RoadPrefabs[2];
                }
                if (roadD.N && !roadD.E && roadD.S && roadD.W)
                {
                    return RoadPrefabs[2];
                }
                if (roadD.N && roadD.E && !roadD.S && roadD.W)
                {
                    _yRotationToApply = 90;
                    return RoadPrefabs[2];
                }
                if (roadD.N && roadD.E && roadD.S && !roadD.W)
                {
                    _yRotationToApply = 180;
                    return RoadPrefabs[2];
                }
            }

            //If none are true, return the 4 way piece
            return RoadPrefabs[1];
        }

        public override void OnWorldGridTileCreated(WorldGridTile tile)
        {
            worldTile = tile;
            ApplyModelRotation();

            //Tell any adjacent road tiles to check and update their models.
            GridTile[] adjacentTiles = ParentTile.GetAdjacentGridTiles();

            foreach(GridTile adjtile in adjacentTiles)
            {
                if(adjtile?.Entity is Road)
                {
                    ((Road)adjtile.Entity).RecheckAndApplyRoadModel();
                    Debug.Log("Sent recheck for road!");
                }
            }
        }

        public void ApplyModelRotation()
        {
            //We need to apply any rotations to the model depending on what was calculated earlier
            Vector3 newRot = new Vector3(0, _yRotationToApply);
            worldTile.Model.transform.eulerAngles = newRot;
            _yRotationToApply = 0;
        }

        /// <summary>
        /// Recheck if the model for this road should be changed. If soo the correct model will be applied.
        /// </summary>
        public void RecheckAndApplyRoadModel()
        {
            GameObject newModel = GetModel();
            worldTile.UpdateModel(newModel);
            ApplyModelRotation();
        }

        private bool CheckTileForRoad(Vector2Int tilePosition)
        {
            Vector2Int positionToCheck = ParentTile.Position + tilePosition;
            GridTile tile = ParentTile.ParentGridSystem.GetTile(positionToCheck);
            return tile?.Entity is Road;
        }

        private static void LoadRoadPrefabs()
        {
            //Cache and load all road prefabs.
            RoadPrefabs = new GameObject[4];
            RoadPrefabs[0] = Resources.Load<GameObject>(RoadPath + "Road EW");
            RoadPrefabs[1] = Resources.Load<GameObject>(RoadPath + "Road NSEW");
            RoadPrefabs[2] = Resources.Load<GameObject>(RoadPath + "Road NSW");
            RoadPrefabs[3] = Resources.Load<GameObject>(RoadPath + "Road NW");
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
