using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUI : MonoBehaviour
{
    private bool FollowGaze = true;

    public Transform anchor;
    public Transform menuTransform;

    public Tagalong tagAlong;

    private void Start()
    {
        if (!anchor) anchor = Camera.main.transform;
        
        UIManager.Instance.StateChanged += ToggleMove;
    }

    private void ResetGazePosition()
    {
        transform.position = anchor.position;
        Quaternion rot = anchor.transform.rotation;
        rot.x = 0;
        transform.rotation = rot;
       // menuTransform.localPosition = offset;
    }

    private void MoveToGaze()
    {
        transform.position = anchor.position;


      // // if (Quaternion.Angle(transform.rotation, anchor.transform.rotation) > angleThreshold)
      ////  {
      //      RotateGaze = true;
      //  }
       
        //menuTransform.localPosition = offset;
    }

    private void RotateToGaze()
    {
        //Quaternion targetRot = Quaternion.Lerp(transform.rotation, anchor.transform.rotation, lerpSpeed * Time.deltaTime);
        //targetRot.x = 0;
        //transform.rotation = targetRot;


        //if (Quaternion.Angle(transform.rotation, anchor.transform.rotation) < angleThreshold) RotateGaze = false;
    }

    public void MoveToTile(WorldGridTile tile)
    {
        FollowGaze = false;

        float height = GetHeight(tile) + 0.025f;

        Vector3 pos = tile.transform.position;
        pos.y += height;

        menuTransform.localPosition = Vector3.zero;
        transform.position = pos;
    }


    float GetHeight(WorldGridTile tile)
    {
        float height = 0f;
        Vector2Int pos = tile.Position;
        WorldGrid grid = tile.ParentGrid;


        for(int i = pos.x - 1; i < pos.x + 2; i++)
        {
            for(int j = pos.y -1; j < pos.y + 2; j++)
            {
                WorldGridTile adjTile = grid.GetTile(i, j);
                if (adjTile == null) continue;

                if (adjTile.Model)
                {
                    if (adjTile.Model.GetComponent<MeshRenderer>())
                    {
                        height = (adjTile.Model.GetComponent<MeshRenderer>().bounds.size.y > height) ? adjTile.Model.GetComponent<MeshRenderer>().bounds.size.y : height;
                    }
                    else
                    {
                        height = (adjTile.Model.GetComponentInChildren<MeshRenderer>().bounds.size.y > height) ? adjTile.Model.GetComponentInChildren<MeshRenderer>().bounds.size.y : height;
                    }
                }
            }
        }
        return height;
    }
    private void ToggleMove(int state)
    {
        FollowGaze = (state == 1) ? true : false;

        tagAlong.enabled = FollowGaze;
    }

}

