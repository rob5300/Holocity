using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildTool;
using HoloToolkit.Unity.InputModule;

public class TapGesture : MonoBehaviour, IInputClickHandler {

    GazeManager gazeManager;
	// Use this for initialization
	void Start () {
        gazeManager = GetComponent<GazeManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
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
