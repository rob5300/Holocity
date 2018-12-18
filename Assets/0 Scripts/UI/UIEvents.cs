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
        CreateGridButton.OnButtonClicked += CreateGridInvoke;
        OnDefaultCreateButton.OnButtonClicked += CreateDefaultGridInvoke;
    }

    private void CreateGridInvoke(GameObject go)
    {
        OnCreateGrid?.Invoke();
    }

    private void CreateDefaultGridInvoke(GameObject go)
    {
        OnDefaultCreateGrid?.Invoke();
    }
}
