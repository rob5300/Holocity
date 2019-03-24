using UnityEngine;
using HoloToolkit.Unity.InputModule;
using Infrastructure.Grid.Entities;
using Infrastructure.Grid;

public class MoveGesture : MonoBehaviour, IManipulationHandler
{
    public float offset = 0.1f;
    private WorldGridTile _tileParent;
    private Vector3 _startPosition;
    private float _startRotation;
    private VoiceCommands _voiceCommand;
    private HandDetection _handDetection;
    private Transform _cameraTransform;
    private FocusHighlighter _currentFocus;
    private GesturesManager _gesturesManager;

    private GridTile gridTile;


    void Start()
    {
        _tileParent = GetComponentInParent<WorldGridTile>();
        _handDetection = FindObjectOfType<HandDetection>();
        _cameraTransform = Camera.main.transform;
        _gesturesManager = Camera.main.GetComponent<GesturesManager>();

        gridTile = GetComponentInParent<WorldGridTile>().GridTileCounterpart;
    }

    void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
    {
        //We now also check if the entity is allowed to be moved.
        if (_gesturesManager.IsNavigating || !_tileParent.GridTileCounterpart.Entity.CanBeMoved) return;

        InputManager.Instance.PushModalInputHandler(gameObject);

        _startPosition = transform.position;
        _startPosition += Vector3.up * offset;
        
        _startRotation = transform.rotation.y;
        transform.position = _startPosition;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        //Call the move start event on the tile entity
        gridTile?.Entity?.OnMoveStart();

        eventData.Use();
    }

    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (!InputManager.Instance.CheckModalInputStack(gameObject) || _gesturesManager.IsNavigating) return;

        transform.position = _startPosition + eventData.CumulativeDelta;
        Quaternion rot = Quaternion.LookRotation(_cameraTransform.forward);
        rot.z = rot.x = 0;
        rot.y += _startRotation;
        transform.rotation = rot;

        MoveBuilding();

        eventData.Use();

    }

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
        TileEntity entity = gridTile.Entity;
        Vector2Int oldPosition = entity.ParentTile.Position;

        SwapBuilding();

        //Call the move start event on the tile entity
        if (oldPosition != entity.ParentTile.Position) entity.OnMoveComplete();
        else entity.OnMoveCancelled();

        eventData.Use();
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        TileEntity entity = gridTile.Entity;
        Vector2Int oldPosition = entity.ParentTile.Position;

        SwapBuilding();

        //Call the move start event on the tile entity
        if (oldPosition != entity.ParentTile.Position) entity.OnMoveComplete();
        else entity.OnMoveCancelled();

        eventData.Use();
    }

    void SwapBuilding()
    {
        if (_gesturesManager.IsNavigating) return;

        InputManager.Instance.PopModalInputHandler();

        _tileParent.SnapRotation(transform);
        _tileParent.AttemptBuildingSwap(transform.position);

        transform.localPosition = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("Hologram");
    }

    void MoveBuilding()
    {
       if (_gesturesManager.IsNavigating || !InputManager.Instance.CheckModalInputStack(gameObject)) return;
        
        LayerMask layerMask = LayerMask.NameToLayer("Hologram");
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, layerMask))
        {
            FocusHighlighter focus = hit.transform.GetComponent<FocusHighlighter>();

            if (focus)
            {
                if (focus != _currentFocus)
                {
                    if (_currentFocus)
                        _currentFocus.ResetColour();

                    focus.HighlightObject();
                    _currentFocus = focus;
                }
            }
        }
    }

}