﻿using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGridMoveButton : MonoBehaviour ,IManipulationHandler
{
    public WorldGrid GridParent;

    private Vector3 _startPosition;
    private Vector3 _gridStartPosition;
/*
    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        //transform.SetParent(GridParent.GridContainer.transform);
        //InputManager.Instance.PushModalInputHandler(gameObject);

    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        //transform.SetParent(GridParent.GridContainer.transform);
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        Debug.Log("Move button Start");
        //transform.SetParent(null);
        _startPosition = transform.parent.position;
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        Debug.Log("Move button Update");
        transform.position = _startPosition + eventData.CumulativeDelta;
    }
*/


    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        transform.SetParent(GridParent.GridContainer.transform);

        eventData.Use();

    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        transform.SetParent(GridParent.GridContainer.transform);
        eventData.Use();

    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
		Debug.Log("Move button Start");
        transform.SetParent(null);

        InputManager.Instance.PushModalInputHandler(gameObject);

        _startPosition = transform.position;
        _gridStartPosition = GridParent.GridContainer.transform.position;
        eventData.Use();

    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
		Debug.Log("Move button Update");
        transform.position = _startPosition + eventData.CumulativeDelta;
        GridParent.GridContainer.transform.position = _gridStartPosition + eventData.CumulativeDelta;
        eventData.Use();

    }

}