using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildTool;
using HoloToolkit.Unity.InputModule;

public class GestureHandler : MonoBehaviour, IInputClickHandler, IFocusable, INavigationHandler, IManipulationHandler {

    public float rotationSpeed = 5;
    private Vector3 originalPos = Vector3.zero;
    private Vector3 raisedPos = Vector3.zero;

	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    //will bring up menu
    public void OnInputClicked(InputClickedEventData eventData)
    {

    }

    //Focus possibly removed.
    void IFocusable.OnFocusEnter()
    {

    }

    void IFocusable.OnFocusExit()
    {
        
    }

    //Rotating House
    void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
    {
        //InputManager.Instance.PushModalInputHandler(gameObject);
    }

    void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
    {
        if (InputManager.Instance.CheckModalInputStack(gameObject))
        {
        //  transform.Rotate(new Vector3(0, eventData.NormalizedOffset.x * rotationSpeed, 0));
        }
    }

    void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
       // Tools.SnapRotation(transform);
    }

    void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
       // Tools.SnapRotation(transform);
    }

    //Moving House
    void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
        originalPos = transform.position;
        raisedPos = originalPos;
        raisedPos.y += 0.2f;
    }

    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
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

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        Tools.MoveBuilding(transform, originalPos);
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        Tools.ResetBuildingPos(transform, originalPos);
    }

}
