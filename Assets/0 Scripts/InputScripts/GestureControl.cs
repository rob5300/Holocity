using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class GestureControl : MonoBehaviour, IInputClickHandler, INavigationHandler, IManipulationHandler, ISpeechHandler
{

    int mode = 2;
    private float RotationSensitivity = 10.0f;
    private Vector3 manipulationOriginalPosition = Vector3.zero;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if(mode == 1)
            gameObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }


    void ISpeechHandler.OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        Debug.Log(eventData.RecognizedText);
        var voiceCommand = eventData.RecognizedText.ToLower();
        switch (voiceCommand)
        {
            case "move":
                {
                    mode = 0;
                    break;
                }
            case "color":
                {
                    mode = 1;
                    break;
                }
            case "rotate":
                {
                    mode = 2;
                    break;
                }
            default:
                break;
        }
    }

    void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
    {
        if (mode == 2)
        {
            float rotationX = eventData.NormalizedOffset.x * RotationSensitivity;
            transform.Rotate(new Vector3(0, -1 * rotationX, 0));
        }
    }

    void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
    {
        if (mode == 0)
        {
            InputManager.Instance.PushModalInputHandler(gameObject);

            manipulationOriginalPosition = transform.position;
        }
    }

    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (mode == 0)
        {
            transform.position = manipulationOriginalPosition + eventData.CumulativeDelta * 10;
        }
    }

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

    }
}
