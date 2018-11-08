using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class Highlighter : MonoBehaviour, IFocusable {


    //This will later be adjusted to toggle shader
    void IFocusable.OnFocusEnter()
    {
        GetComponent<Renderer>().material.color = Color.green;
    }

    void IFocusable.OnFocusExit()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }
}
