using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildTool;
using HoloToolkit.Unity.InputModule;

public class TapGesture : MonoBehaviour, IInputClickHandler {

    public BuildToolUI buildUI;
    GazeManager gazeManager;
    

	void Start ()
    {
        gazeManager = GetComponent<GazeManager>();  
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        
        if (eventData.used || !gazeManager.HitObject) return;

        eventData.Use();
        
        if (gazeManager.HitObject.GetComponent<FocusHighlighter>())
        {
           WorldGridTile tile = gazeManager.HitObject.transform.parent.GetComponent<WorldGridTile>();
            
            buildUI.MoveUI(tile);            
        }
    }

}
