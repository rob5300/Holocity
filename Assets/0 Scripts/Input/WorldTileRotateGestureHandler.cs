using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class WorldTileRotateGestureHandler : MonoBehaviour, INavigationHandler {

    public float RotationSpeed = 3;

    private WorldGridTile _tileParent;
    private Quaternion _startRotation;
    private VoiceGestureControl _voiceCommand;

    void Start()
    {
        _tileParent = GetComponentInParent<WorldGridTile>();
        _voiceCommand = InputManager.Instance.transform.GetComponent<VoiceGestureControl>();
        
    }
    
    void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
    {
        if (!_voiceCommand.IsNavigating) return;

        InputManager.Instance.PushModalInputHandler(gameObject);
        _startRotation = transform.rotation;

        eventData.Use();
    }

    void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
    {
        if (InputManager.Instance.CheckModalInputStack(gameObject) && _voiceCommand.IsNavigating)
        {
            transform.Rotate(new Vector3(0, eventData.NormalizedOffset.x * -RotationSpeed, 0));


            eventData.Use();
        }
    }

    void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
    {
        if (!InputManager.Instance.CheckModalInputStack(gameObject) || !_voiceCommand.IsNavigating) return;


        _tileParent.RotateBuilding(transform);
        InputManager.Instance.PopModalInputHandler();

        eventData.Use();
    }

    void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
    {
        if (!_voiceCommand.IsNavigating) return;

        InputManager.Instance.PopModalInputHandler();
        transform.rotation = _startRotation;

        eventData.Use();
    }
    
}
