using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildTool;
using HoloToolkit.Unity.InputModule;

public class TapGesture : MonoBehaviour, IInputClickHandler {

    GazeManager gazeManager;
	
	void Start () {
        gazeManager = GetComponent<GazeManager>();
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (gazeManager.HitObject.GetComponent<Highlighter>())
        {
           WorldGridTile tile = gazeManager.HitObject.transform.parent.GetComponent<WorldGridTile>();
           Tools.SpawnBuilding(tile.Position);
        }
    }

}
