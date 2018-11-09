using UnityEngine;
using System;
using Infrastructure.Grid;
using Infrastructure.Grid.Entities.Buildings;
using HoloToolkit.Unity.InputModule;
using BuildTool;

[SelectionBase]
public class WorldGridTile : MonoBehaviour {

    public WorldGrid ParentGrid;
    public GridTile GridTileCounterpart { get {
            return ParentGrid.GridSystem.GetTile(Position);
        } }
    public Vector2Int Position;
    public GameObject TileBorder;
    public GameObject Model;

    public event Action OnInteract;
    public event Action OnHoverBegin;
    public event Action OnHoverEnd;
    public event Action OnMoveBegin;
    public event Action<WorldGridTile> OnMoveEnd;
    public event Action OnMoveFail;
    public event Action OnTileDestroy;

    public void Initialize(Vector2Int tileposition, WorldGrid parent)
    {
        Position = tileposition;
        ParentGrid = parent;
        TileBorder = Instantiate(Game.CurrentSession.Cache.TileBorder, transform);

        TileBorder.AddComponent<Highlighter>();
        TileBorder.layer = LayerMask.NameToLayer("Hologram");
    }

    public void OnDestroy()
    {
        OnTileDestroy?.Invoke();
    }

    internal void AddBuildingFromGridSystem(Building building)
    {
        TileBorder.SetActive(false);
        Model = Instantiate(building.BuildingPrefab, transform);

        //Add gesture forwarder and sub to events.
        GestureEventForwarder forwarder = Model.AddComponent<GestureEventForwarder>();
        forwarder.E_ManipulationStarted += MoveStarted;
        forwarder.E_ManipulationUpdated += MoveUpdated;
        forwarder.E_ManipulationCompleted += MoveCompleted;
        forwarder.E_ManipulationCanceled += MoveCanceled;

        //Set the layer mask.
        Model.layer = LayerMask.NameToLayer("Hologram");

        //Tell the building that this tile exists now.
        building.OnWorldGridTileCreated(this);
    }

    public float rotationSpeed = 5;
    private Vector3 originalPos = Vector3.zero;
    private Vector3 raisedPos = Vector3.zero;

    private void MoveStarted(ManipulationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);

        originalPos = transform.position;
        raisedPos = originalPos;
        raisedPos.y += 0.15f;
    }

    private void MoveUpdated(ManipulationEventData eventData)
    {
        if (!InputManager.Instance.CheckModalInputStack(gameObject))
        {
            return;
        }
        //Haven't able to test so will have to rely on moving buildings left or right.
        Vector3 handPos = eventData.CumulativeDelta;
        Vector3 newPos = raisedPos + handPos;
        newPos.y = raisedPos.y;

        transform.position = newPos;
    }


    private void MoveCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        if (!AttemptBuildingSwap())
        {
            transform.position = originalPos;
        }
    }

    private void MoveCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        Tools.ResetBuildingPos(transform);
    }

    private bool AttemptBuildingSwap()
    {
        LayerMask layerMask = LayerMask.NameToLayer("Hologram");
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, layerMask))
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
}
