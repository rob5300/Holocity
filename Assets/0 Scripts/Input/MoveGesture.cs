using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildTool;
using HoloToolkit.Unity.InputModule;

public class MoveGesture : MonoBehaviour, IManipulationHandler
{
    public float offset = 0.15f;
    private WorldGridTile _tileParent;
    private Vector3 _startPosition;
    private VoiceGestureControl _voiceCommand;
    private HandDetection _handDetection;
    private Transform _cameraTransform;
    private FocusHighlighter _currentFocus;
    void Start()
    {
        _tileParent = GetComponentInParent<WorldGridTile>();
        _voiceCommand = FindObjectOfType<VoiceGestureControl>();
        _handDetection = FindObjectOfType<HandDetection>();
        _cameraTransform = Camera.main.transform;
    }
    
    void Update()
    {
        MoveBuilding();
    }


    void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
    {
        if (_voiceCommand.IsNavigating) return;

        InputManager.Instance.PushModalInputHandler(gameObject);

        _startPosition = _handDetection.GetHandPos(eventData.SourceId);
        _startPosition += _cameraTransform.forward * offset;
        transform.position = _startPosition;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        
        eventData.Use();
    }

    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (!InputManager.Instance.CheckModalInputStack(gameObject) || _voiceCommand.IsNavigating) return;

        transform.position = _startPosition + eventData.CumulativeDelta;
       // transform.rotation = _cameraTransform.rotation;

        eventData.Use();

    }

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
        SwapBuilding();
        eventData.Use();
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        SwapBuilding();
        eventData.Use();
    }

    void SwapBuilding()
    {
        if (_voiceCommand.IsNavigating) return;

        InputManager.Instance.PopModalInputHandler();

        _tileParent.AttemptBuildingSwap(transform.position);

        transform.localPosition = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("Hologram");
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