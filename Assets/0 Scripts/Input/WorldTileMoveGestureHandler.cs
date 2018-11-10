using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildTool;
using HoloToolkit.Unity.InputModule;

public class WorldTileMoveGestureHandler : MonoBehaviour, IManipulationHandler {

    public float RotationSpeed = 5;
    public Vector3 MoveOffset = new Vector3(0, 0.15f, 0);

    private WorldGridTile _tileParent;
    private Vector3 _startPosition;

    public void Start()
    {
        _tileParent = GetComponentInParent<WorldGridTile>();
    }

    //When the move gesture begins
    void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);

        _startPosition = transform.localPosition;
        transform.position += MoveOffset;

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (!InputManager.Instance.CheckModalInputStack(gameObject)) return;

        Vector3 gestureMovemnt = eventData.CumulativeDelta;
        gestureMovemnt.y = 0;
        transform.position += gestureMovemnt;
    }

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        transform.position -= MoveOffset;
        _tileParent.AttemptBuildingSwap(transform.position + MoveOffset);
        transform.localPosition = _startPosition;
        gameObject.layer = LayerMask.NameToLayer("Hologram");
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        transform.localPosition = _startPosition;
    }

}
