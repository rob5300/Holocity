using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.XR.WSA.Input;
using System;

public class HandDetection : MonoBehaviour{
    
    GameObject leftHand;
    Vector3 leftHandPos;
    uint leftHandSourceID;

    GameObject rightHand;
    Vector3 rightHandPos;
    uint rightHandSourceID;


    private void Awake()
    {
        InteractionManager.InteractionSourceDetected += SourceFound;
        InteractionManager.InteractionSourceLost += SourceLost;
        InteractionManager.InteractionSourceUpdated += SourceUpdated;
    }

    private void SourceFound(InteractionSourceDetectedEventArgs obj)
    {
        /*
        if (rightHand == null && obj.state.source.kind.ToString() == "Hand")
        {
            rightHand = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            rightHand.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            rightHandSourceID = obj.state.source.id;

            if (obj.state.sourcePose.TryGetPosition(out rightHandPos))
                rightHand.transform.position = rightHandPos;

            if (rightHandPos != Vector3.zero)
                Debug.Log("Right Hand: " + rightHandSourceID + "  ::  " + rightHandPos);
        }
        else if (leftHand == null && obj.state.source.kind.ToString() == "Hand")
        {
            leftHand = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            leftHand.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            leftHandSourceID = obj.state.source.id;

            if (obj.state.sourcePose.TryGetPosition(out leftHandPos))
                leftHand.transform.position = leftHandPos;

            if (leftHandPos != Vector3.zero)
                Debug.Log("Left Hand: " + rightHandSourceID + " :: " + rightHandPos);
        }
        */
    }

    private void SourceLost(InteractionSourceLostEventArgs obj)
    {
        if(obj.state.source.id == rightHandSourceID)
        {
            Destroy(rightHand);
            rightHandPos = Vector3.zero;
            rightHandSourceID = 0;
        }
        else if(obj.state.source.id == leftHandSourceID)
        {
            Destroy(leftHand);
            leftHandPos = Vector3.zero;
            leftHandSourceID = 0;
        }
    }

    private void SourceUpdated(InteractionSourceUpdatedEventArgs obj)
    {
        if (rightHand && rightHandSourceID == obj.state.source.id)
        {
            if (obj.state.sourcePose.TryGetPosition(out rightHandPos))
                rightHand.transform.position = rightHandPos;
        }

        if (leftHand && leftHandSourceID == obj.state.source.id)
        {
            if (obj.state.sourcePose.TryGetPosition(out leftHandPos))
                leftHand.transform.position = leftHandPos;
        }
    }

   
}
