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
            
        }

        public static void ResetBuildingPos(Transform transform)
        {
            transform.localPosition = Vector3.zero;
        }

        public static void SpawnBuilding(Vector2Int position)
        {
            //gonna have to check what grid to add to.
            Game.CurrentSession.City.GetGrid(0).AddBuildingToTile(position.x, position.y, new House());
        }
        
    }
}