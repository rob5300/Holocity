using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class GazeAction : MonoBehaviour, IInputClickHandler {

    public GazeManager gazeManager;
    public Camera main;
    public GameObject prefab;

	// Use this for initialization
	void Start ()
    {
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (gazeManager.HitObject == null)
        {
            Vector3 pos = main.transform.position + main.transform.forward * 5;
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }

}
