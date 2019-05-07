using UnityEngine;
using System;
using Infrastructure.Grid;
using Infrastructure.Grid.Entities;

[SelectionBase]
public class WorldGridTile : MonoBehaviour {

    public WorldGrid ParentGrid;
    public GridTile GridTileCounterpart { get {
            return ParentGrid.GridSystem.GetTile(Position);
        } }
    public Vector2Int Position;
    public GameObject TileBorder;
    public GameObject Model;

    public event Action OnTileDestroy;

    public void Initialize(Vector2Int tileposition, WorldGrid parent)
    {
        Position = tileposition;
        ParentGrid = parent;
        TileBorder = Instantiate(Game.CurrentSession.Cache.TileBorder, transform);

        TileBorder.AddComponent<FocusHighlighter>();
        TileBorder.layer = LayerMask.NameToLayer("Hologram");
    }

    public void OnDestroy()
    {
        OnTileDestroy?.Invoke();
    }

    public void AddModelToTile(TileEntity tileEnt)
    {
        TileBorder.SetActive(false);
        Model = Instantiate(tileEnt.GetModel(), transform);

        
        //Reset the position of the model to 0.
        Model.transform.localPosition = Vector3.zero;

        //Add Gesture Components to Buildings
        Model.AddComponent<MoveGesture>();
        Model.AddComponent<RotateGesture>();

        //Set the layer mask.
        Model.layer = LayerMask.NameToLayer("Hologram");

        //enable any sounds
        if (Model.GetComponent<AudioSource>()) Model.GetComponent<AudioSource>().Play();

        //(Model.GetComponent<MeshRenderer>() ?? Model.GetComponentInChildren<MeshRenderer>()).material.color = Color.white;
        //Tell the Tile entity that this tile exists now.
        tileEnt.OnWorldGridTileCreated(this);
    }

    public void UpdateModel(GameObject newModel)
    {
        Destroy(Model);
        Model = Instantiate(newModel, transform);
        Model.transform.localPosition = Vector3.zero;

        //Add Gesture Components to Buildings
        Model.AddComponent<MoveGesture>();
        Model.AddComponent<RotateGesture>();
        Model.AddComponent<FocusHighlighter>();

        //enable any sounds
        if (Model.GetComponent<AudioSource>()) Model.GetComponent<AudioSource>().Play();

        //Set the layer mask.
        Model.layer = LayerMask.NameToLayer("Hologram");
    }

    public void RemoveModel()
    {
        Destroy(Model);
        Model = null;
        TileBorder.SetActive(true);
    }

    public bool AttemptBuildingSwap(Vector3 checkPosition)
    {
        //Will need to update to work with multiple grids.

        LayerMask layerMask = LayerMask.NameToLayer("Hologram");
        RaycastHit hit;

        if (Physics.Raycast(checkPosition, -Vector3.up, out hit, layerMask))
        {
            //Check for WorldGridTile.
            WorldGridTile foundTile = hit.transform.parent.GetComponent<WorldGridTile>() ?? hit.transform.parent.GetComponentInParent<WorldGridTile>();

            //Check if there was a found tile and if the found grid tile shares the same grid id.
            if (foundTile != null && foundTile.ParentGrid.Id == ParentGrid.Id)
            {
                //Try to swap these two tiles around. if it fails we know to reset the positions.
                return ParentGrid.GridSystem.SwapTileEntities(Position, foundTile.Position);
            }
        }
        return false;
    }

    public void SnapRotation(Transform target)
    {
        Vector3 parentForward = target.parent.forward;
        Vector3 parentRight = target.parent.right;
 
        if (Game.CurrentSession.Settings.CurrentTimePeriod == Settings.TimePeriod.Medieval)
        {
           // parentForward = transform.parent.up;
        }

        float dotF = Vector3.Dot(target.forward, parentForward);
        float dotR = Vector3.Dot(target.forward, parentRight);
        Vector3 dir;


        if (Mathf.Abs(dotF) > Mathf.Abs(dotR))
        {
            dir = (parentForward * Mathf.Round(dotF));
            target.rotation = Quaternion.LookRotation(dir);

        }
        else if (Mathf.Abs(dotF) < Mathf.Abs(dotR))
        {
            dir = (parentRight * Mathf.Round(dotR));
            target.rotation = Quaternion.LookRotation(dir);
        }
    }
}

