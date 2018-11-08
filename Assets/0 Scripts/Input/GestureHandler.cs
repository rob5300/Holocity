using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildTool;
using HoloToolkit.Unity.InputModule;

public class GestureHandler : MonoBehaviour, IInputClickHandler, IFocusable, INavigationHandler {

    public float rotationSpeed;

	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {

    }

    void IFocusable.OnFocusEnter()
    {

    }

    void IFocusable.OnFocusExit()
    {
        
    }

    void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
    {
        if (InputManager.Instance.CheckModalInputStack(gameObject))
        {
          transform.Rotate(new Vector3(0, eventData.NormalizedOffset.x * rotationSpeed, 0));
        }
    }

    void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        Tools.SnapRotation(transform);
    }

    void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        Tools.SnapRotation(transform);
    }
}
