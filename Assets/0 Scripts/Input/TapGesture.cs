using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildTool;
using HoloToolkit.Unity.InputModule;

public class TapGesture : MonoBehaviour, IInputClickHandler {


    public BuildToolUI buildToolUI;

    GazeManager gazeManager;
    float timeofuse = -999;

	void Start () {
        gazeManager = GetComponent<GazeManager>();
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if(timeofuse + 0.1 > Time.time) return;

        timeofuse = Time.time;

        eventData.Use();

        if (gazeManager.HitObject && gazeManager.HitObject.GetComponent<FocusHighlighter>())
        {
           WorldGridTile tile = gazeManager.HitObject.transform.parent.GetComponent<WorldGridTile>();

           //Tools.SpawnBuilding(tile.Position);

            buildToolUI.MoveUI(tile);

        }
        else
        {
            //turn off UI if player clicks away.
        }
    }

}
