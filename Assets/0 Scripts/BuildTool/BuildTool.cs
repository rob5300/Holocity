using UnityEngine;
using Infrastructure.Grid.Entities;

namespace BuildTool
{
    public static class Tools
    {
        private static System.Random r;

        private static int buildingNum = 0;

        static Tools(){
            r = new System.Random();
        }

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
            
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, layerMask))
            {
                //checks if we are placing on a building or gridslot
                if (hit.transform.parent.GetComponent<WorldGridTile>())
                {
                    
                    Vector2Int a = transform.parent.GetComponent<WorldGridTile>().Position;
                    Vector2Int b = hit.transform.parent.GetComponent<WorldGridTile>().Position;

                    bool check = Game.CurrentSession.City.GetGrid(0).SwapTileEntities(a, b);
                    ResetBuildingPos(transform);
                }
                else
                {
                   ResetBuildingPos(transform);
                }
            }
            else
            {
                ResetBuildingPos(transform);
            }
        }

        public static void ResetBuildingPos(Transform transform)
        {
            transform.localPosition = Vector3.zero;
        }

        public static void SpawnBuilding(Vector2Int position, TileEntity ent)
        {
           Game.CurrentSession.City.GetGrid(0).AddTileEntityToTile(position.x, position.y, ent);

        }

    }
}