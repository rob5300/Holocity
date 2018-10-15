using System;
using Microsoft.MixedReality.Toolkit.SDK.Input;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class PlayerGestureInput : MonoBehaviour {

    private Camera _camera;
    private GestureRecognizer _gestureRecognizer;
    private Ray raycastRay;

    public void Start()
    {
        //Cached because apparantly this is not cached in unity
        _camera = Camera.main;
        _gestureRecognizer = new GestureRecognizer();
        
        _gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold | GestureSettings.DoubleTap);
        //Subscribe to the gesture events:
        _gestureRecognizer.Tapped += GestureTapped;
    }

    private void GestureTapped(TappedEventArgs obj)
    {
        throw new NotImplementedException();
    }

    void Update ()
    {
        
    }

    public void OnDestroy()
    {
        //Unsubscribe from gesture events:
    }
}
