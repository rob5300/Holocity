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
            float dotF = Vector3.Dot(target.forward, target.root.forward);
            float dotR = Vector3.Dot(target.forward, target.root.right);
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

        public static void MoveBuilding()
        {
            //shoot raycast down. check if there is a tile,
            //if not cancel else swap or place


            // AddBuildingToTile(int x, int y, Building building)
        }
    }
}