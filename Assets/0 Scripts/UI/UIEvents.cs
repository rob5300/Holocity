using HoloToolkit.Unity.Buttons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIEvents : MonoBehaviour {

    public CompoundButton CreateGridButton;
    public UnityEvent OnCreateGrid;
    public CompoundButton OnDefaultCreateButton;
    public UnityEvent OnDefaultCreateGrid;

    public void Start()
    {
        CreateGridButton.OnButtonClicked += (GameObject ob) => { OnCreateGrid?.Invoke(); };
        OnDefaultCreateButton.OnButtonClicked += (GameObject ob) => { OnDefaultCreateGrid?.Invoke(); };
    }

}
