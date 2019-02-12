using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.XR.WSA.Input;
using System;

public class HandDetection : MonoBehaviour{
    
    private struct Hand
    {
        public GameObject obj;
        public Vector3 pos;
        public uint ID;
        public bool isPressed;
    }

    Hand handOne = new Hand();
    Hand handTwo = new Hand();

    Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
    

    private void Awake()
    {
        InteractionManager.InteractionSourceDetected += SourceFound;
        InteractionManager.InteractionSourceLost += SourceLost;
        InteractionManager.InteractionSourceUpdated += SourceUpdated;
    }
    
    public Vector3 GetHandPos(uint id)
    {
        Vector3 handPos = Vector3.zero;

        if (true)//handOne.obj )//&& handOne.isPressed)// && handOne.ID == id)
        {
            handPos = handOne.pos;
        }
        else if (handTwo.obj && handTwo.isPressed && handTwo.ID == id)
        {
            handPos = handTwo.pos;
        }

        return handPos;
    }

    private void SourceFound(InteractionSourceDetectedEventArgs obj)
    {
        if (obj.state.source.kind.ToString() != "Hand") return;

        if (handOne.obj == null)
        {
            handOne.obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            handOne.obj.transform.localScale = scale;
            handOne.ID = obj.state.source.id;

            if (obj.state.sourcePose.TryGetPosition(out handOne.pos))
                handOne.obj.transform.position = handOne.pos;
        }
        else if (handTwo.obj == null)
        {
            handTwo.obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            handTwo.obj.transform.localScale = scale;
            handTwo.ID = obj.state.source.id;

            if (obj.state.sourcePose.TryGetPosition(out handTwo.pos))
                handTwo.obj.transform.position = handTwo.pos;
        }
    }

    private void SourceLost(InteractionSourceLostEventArgs obj)
    {
        if(obj.state.source.id == handOne.ID)
        {
            Destroy(handOne.obj);
            handOne.pos = Vector3.zero;
            handOne.ID = 0;
        }
        else if(obj.state.source.id == handTwo.ID)
        {
            Destroy(handTwo.obj);
            handTwo.pos = Vector3.zero;
            handTwo.ID = 0;
        }
    }

    private void SourceUpdated(InteractionSourceUpdatedEventArgs obj)
    {
        if (handOne.obj && handOne.ID == obj.state.source.id)
        {
            if (obj.state.sourcePose.TryGetPosition(out handOne.pos))
                handOne.obj.transform.position = handOne.pos;

            handOne.isPressed = obj.state.selectPressed;
        }

        if (handTwo.obj && handTwo.ID == obj.state.source.id)
        {
            if (obj.state.sourcePose.TryGetPosition(out handTwo.pos))
                handTwo.obj.transform.position = handTwo.pos;

            handTwo.isPressed = obj.state.selectPressed;
        }
    }

    
}
