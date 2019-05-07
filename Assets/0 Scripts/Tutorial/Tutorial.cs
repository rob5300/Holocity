using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour {


    TextMeshPro textMesh;
    
	void Start () {
        textMesh = GetComponent<TextMeshPro>();
	}
	
	void Update ()
    {
        if (GazeManager.Instance.HitObject) CheckTarget();
	}

    void CheckTarget()
    {

        if (GazeManager.Instance.HitObject.GetComponentInParent<WorldGridTile>())
        {
            Vector3 pos = GazeManager.Instance.HitObject.transform.position;
            pos.y += 0.3f;
            transform.position = pos;

            textMesh.text = "TAP on tile to start!";
        }


    }

}
