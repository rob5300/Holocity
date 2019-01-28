using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildEnums;
using Infrastructure.Grid;
using Infrastructure.Grid.Entities.Buildings;

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

        public static void SpawnBuilding(Vector2Int position)
        {
            Game.CurrentSession.City.GetGrid(0).AddTileEntityToTile(position.x, position.y, GetRandomBuilding());
            buildingNum++;
            if (buildingNum > 3) buildingNum = 0;
        }

        private static Building GetRandomBuilding()
        {
            switch (buildingNum)
            {
                case 0:
                    return new House();
                case 1:
                    return new Modern_CityBuildings();
                case 2:
                    return new Future_House();
                case 3:
                    return new Old_Church();
                default:
                    return new House();
            }
        }

        public static void SpawnBuilding(Vector2Int position, ModernBuildings building)
        {
           Game.CurrentSession.City.GetGrid(0).AddTileEntityToTile(position.x, position.y, GetBuilding(building));

        }

        public static Building GetBuilding(ModernBuildings building)
        {
            switch (building)
            {
                case ModernBuildings.House:
                    return new House();
                case ModernBuildings.Future_House:
                    return new Future_House();
                case ModernBuildings.Modern_CityBuildings:
                    return new Modern_CityBuildings();
                case ModernBuildings.Old_Church:
                    return new Old_Church();

                default: return new House();
            }
        }
    }
}