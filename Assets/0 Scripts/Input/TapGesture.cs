using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildTool;
using HoloToolkit.Unity.InputModule;

public class TapGesture : MonoBehaviour, IInputClickHandler {


    public BuildToolUI buildToolUI;

    GazeManager gazeManager;

	void Start () {
        gazeManager = GetComponent<GazeManager>();
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if(eventData.used) return;

        eventData.Use();

        if (gazeManager.HitObject && gazeManager.HitObject.GetComponent<FocusHighlighter>())
        {
           WorldGridTile tile = gazeManager.HitObject.transform.parent.GetComponent<WorldGridTile>();

           //Tools.SpawnBuilding(tile.Position);

            buildToolUI.MoveUI(tile);
            
            Debug.Log("NOTHING HIT");
        }
        else
        {
            Debug.Log("NOTHING HIT");
        }
    }

}
