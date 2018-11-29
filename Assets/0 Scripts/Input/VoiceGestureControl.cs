using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class VoiceGestureControl : MonoBehaviour, ISpeechHandler
{
    //may change later to account for other modes.
    public bool IsNavigating { get; private set; }


    void ISpeechHandler.OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        Debug.Log(eventData.RecognizedText);

        //makes sure player is not in the middle of a gesture.
        if (InputManager.Instance.CheckModalInputStack(gameObject)) return;

        if (eventData.RecognizedText.Equals("Move Building"))
        {
			Debug.Log("VOICE: Moving");
            IsNavigating = false;
        }
        else if (eventData.RecognizedText.Equals("Rotate Building"))
        {
			Debug.Log("VOICE: Rotating");
            IsNavigating = true;
        }
		else{
			Debug.Log("VOICE: something else ???:" + eventData.RecognizedText);
		}

    }
}
