using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Infrastructure.Grid;
using Infrastructure.Grid.Entities.Buildings;

namespace BuildTool
{
    public static class Tools
    {
        public static void SnapRotation(Transform target)
        {
            float dotF = Vector3.Dot(target.up, target.root.forward);
            float dotR = Vector3.Dot(target.up, target.root.right);
            Quaternion dir;

            if (Mathf.Abs(dotF) >= Mathf.Abs(dotR))
            {
                dir = Quaternion.LookRotation(target.root.forward * Mathf.Round(dotF), Vector3.up);
                target.rotation = dir;
            }
            else
            {
                dir = Quaternion.LookRotation(target.root.right * Mathf.Round(dotR), Vector3.up);
                target.rotation = dir;
            }
        }

        public static void MoveBuilding(Transform transform, Vector3 pos)
        {
            LayerMask layerMask = LayerMask.NameToLayer("Hologram");
            RaycastHit hit;


            //atm building prefab is rotated 90degrees, so have to use forward vector
            if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.forward), out hit, layerMask))
            {
                //checks if we are placing on a building or gridslot
                if (hit.transform.GetComponent<GestureHandler>())
                {
                    //just take the objects so you can assign the locations.
                    Vector2Int a = transform.parent.GetComponent<WorldGridTile>().Position;
                    Vector2Int b = hit.transform.parent.GetComponent<WorldGridTile>().Position;

                    SwapBuilding(a, b);
                }
                else
                {
                    WorldGridTile tile = hit.transform.parent.GetComponent<WorldGridTile>();
                    SpawnBuilding(tile.Position);
                }
            }
            else
            {
                ResetBuildingPos(transform, pos);
            }
        }

        public static void ResetBuildingPos(Transform transform, Vector3 pos)
        {
          //  transform.position = pos;
        }

        public static void SwapBuilding(Vector2Int tileaposition, Vector2Int tilebposition)
        {
            //will check for grid
            bool check = Game.CurrentSession.City.GetGrid(0).SwapTileEntities(tileaposition, tilebposition);
            Debug.Log("Check: " + check);
        }
        public static void SpawnBuilding(Vector2Int position)
        {
            //gonna have to check what grid to add to.
            Game.CurrentSession.City.GetGrid(0).AddBuildingToTile(position.x, position.y, new House());
        }
        
    }
}