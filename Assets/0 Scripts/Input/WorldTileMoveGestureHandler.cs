using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildTool;
using HoloToolkit.Unity.InputModule;

public class WorldTileMoveGestureHandler : MonoBehaviour, IManipulationHandler {

    public float RotationSpeed = 5;
    //not sure about using a vector as it has to be reset after every manipulation and would be annoying to change if we ever wanted to.
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
        //need to also set the MoveOffset to the new position
        //transform.position += MoveOffset;
        MoveOffset = transform.position + MoveOffset;

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (!InputManager.Instance.CheckModalInputStack(gameObject)) return;

        Vector3 gestureMovemnt = eventData.CumulativeDelta;
        gestureMovemnt.y = 0;

        //This line results in accelerated movement hard to control, The MoveOffset acts as an origin point
        //The reason why you can't move the house properly with my version is controller related. 
        //the joystick only gives you x and y... by all means it should work properly in the actual HoloLens 
        //transform.position += gestureMovemnt;

        transform.position = MoveOffset + gestureMovemnt;

    }

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        //Not sure about these two lines, why are you subtracting and then adding?
        //transform.position -= MoveOffset;
        //_tileParent.AttemptBuildingSwap(transform.position + MoveOffset);
        _tileParent.AttemptBuildingSwap(transform.position);

        transform.localPosition = _startPosition;
        gameObject.layer = LayerMask.NameToLayer("Hologram");

        //Using this line to reset the MoveOffset for now
        MoveOffset = new Vector3(0, 0.15f, 0);
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        transform.localPosition = _startPosition;
        //Using this line to reset the MoveOffset for now
        MoveOffset = new Vector3(0, 0.15f, 0);
    }

}
