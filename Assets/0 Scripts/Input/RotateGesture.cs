using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class RotateGesture : MonoBehaviour, INavigationHandler {

    public float RotationSpeed = 3;

    private WorldGridTile _tileParent;
    private Quaternion _startRotation;
    private GesturesManager _gesturesManager;

    void Start()
    {
        _tileParent = GetComponentInParent<WorldGridTile>();
        _gesturesManager = Camera.main.GetComponent<GesturesManager>();
    }
    
    void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
    {
        if (!_gesturesManager.IsNavigating) return;

        InputManager.Instance.PushModalInputHandler(gameObject);
        _startRotation = transform.rotation;

        eventData.Use();
    }

    void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
    {
        if (InputManager.Instance.CheckModalInputStack(gameObject) && _gesturesManager.IsNavigating)
        {
            transform.Rotate(new Vector3(0, eventData.NormalizedOffset.x * -RotationSpeed, 0));


            eventData.Use();
        }
    }

    void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
    {
        if (!InputManager.Instance.CheckModalInputStack(gameObject) || !_gesturesManager.IsNavigating) return;
        
        _tileParent.SnapRotation(transform);
        InputManager.Instance.PopModalInputHandler();

        eventData.Use();
    }

    void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
    {
        if (!_gesturesManager.IsNavigating) return;

        InputManager.Instance.PopModalInputHandler();
        transform.rotation = _startRotation;

        eventData.Use();
    }
}
