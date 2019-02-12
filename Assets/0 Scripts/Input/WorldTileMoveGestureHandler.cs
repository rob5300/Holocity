
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildTool;
using HoloToolkit.Unity.InputModule;

public class WorldTileMoveGestureHandler : MonoBehaviour, IManipulationHandler {

    
    //not sure about using a vector as it has to be reset after every manipulation and would be annoying to change if we ever wanted to.
    public Vector3 MoveOffset = new Vector3(0, 0.15f, 0);

    private WorldGridTile _tileParent;
    private Vector3 _startPosition;
    private VoiceGestureControl _voiceCommand;
    private FocusHighlighter _currentFocus;

    void Start()
    {

        _tileParent = GetComponentInParent<WorldGridTile>();
        _voiceCommand = FindObjectOfType<VoiceGestureControl>();
    }
    void Update()
    {
        MoveBuilding();
    }
    void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
    {
        if (_voiceCommand.IsNavigating) return;

        InputManager.Instance.PushModalInputHandler(gameObject);

        _startPosition = transform.localPosition;
        MoveOffset = transform.position + MoveOffset;

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        eventData.Use();
    }

    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
		
        if (!InputManager.Instance.CheckModalInputStack(gameObject) || _voiceCommand.IsNavigating) return;
        
        Vector3 gestureMovemnt = eventData.CumulativeDelta;
        //gestureMovemnt.y = 0;
        transform.position = MoveOffset + gestureMovemnt;

        eventData.Use();

    }

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
		
        if (_voiceCommand.IsNavigating) return;
         InputManager.Instance.PopModalInputHandler();
         _tileParent.AttemptBuildingSwap(transform.position);

         transform.localPosition = _startPosition;
         gameObject.layer = LayerMask.NameToLayer("Hologram");

        //Using this line to reset the MoveOffset for now
         MoveOffset = new Vector3(0, 0.15f, 0);

        eventData.Use();
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        if (_voiceCommand.IsNavigating) return;

        InputManager.Instance.PopModalInputHandler();
        transform.localPosition = _startPosition;

        //Using this line to reset the MoveOffset for now
        MoveOffset = new Vector3(0, 0.15f, 0);

        eventData.Use();
    }

    void MoveBuilding()
    {
        if (_voiceCommand.IsNavigating || !InputManager.Instance.CheckModalInputStack(gameObject)) return;

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
