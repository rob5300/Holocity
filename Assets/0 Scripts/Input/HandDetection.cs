using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.XR.WSA.Input;
using System;

public class HandDetection : MonoBehaviour{
    
    public GameObject HandPrefab;

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
    
    public Vector3 GetHandPos()
    {
        Vector3 handPos = Vector3.zero;

        if (handOne.obj && handOne.isPressed)
        {
            handPos = handOne.pos;
        }
        else if (handTwo.obj && handTwo.isPressed)
        {
            handPos = handTwo.pos;
        }

        return handPos;
    }

    private void SourceFound(InteractionSourceDetectedEventArgs obj)
    {
        if (obj.state.source.kind.ToString() != "Hand") return;

        InteractionSource  source = obj.state.source;
        

        if (handOne.obj == null)
        {
            handOne.obj = Instantiate(HandPrefab);
            handOne.obj.GetComponent<HandObject.Hand>().handDetection = this;
            handOne.obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            handOne.obj.transform.localScale = scale;
            handOne.ID = obj.state.source.id;

            if (obj.state.sourcePose.TryGetPosition(out handOne.pos))
                handOne.obj.transform.position = handOne.pos;
        }
        else if (handTwo.obj == null)
        {
            handTwo.obj = Instantiate(HandPrefab);
            handTwo.obj.GetComponent<HandObject.Hand>().handDetection = this;

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

            handOne.isPressed = obj.state.anyPressed;
        }

        if (handTwo.obj && handTwo.ID == obj.state.source.id)
        {
            if (obj.state.sourcePose.TryGetPosition(out handTwo.pos))
                handTwo.obj.transform.position = handTwo.pos;

            handTwo.isPressed = obj.state.anyPressed;
        }
    }

    #region Bimanual Gestures
    public void OpenMainMenu(GameObject go)
    {
        if(go == handOne.obj)
           UIManager.Instance.SwitchState(UIManager.MenuState.MainMenu);
    }

    #endregion
}
