using System;
using UnityEngine;

[RequireComponent(typeof(VoiceCommands))]
[RequireComponent(typeof(HandDetection))]
public class GesturesManager : MonoBehaviour {

    public static GesturesManager Instance;



    public VoiceCommands voiceControl { get; private set;}
    public HandDetection handDetection { get; private set;}
    public bool IsNavigating = false;

    private void Awake()
    {
        Instance = this;

        handDetection = GetComponent<HandDetection>();
        voiceControl = GetComponent<VoiceCommands>();
        voiceControl.gesturesManager = this;
    }

    public void SwitchMode(bool navigate)
    {
        IsNavigating = navigate;
    }
}
