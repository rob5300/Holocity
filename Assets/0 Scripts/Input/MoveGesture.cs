using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class MoveGesture : MonoBehaviour, IManipulationHandler
{
    public float offset = 0.15f;
    private WorldGridTile _tileParent;
    private Vector3 _startPosition;
    private float _startRotation;
    private VoiceCommands _voiceCommand;
    private HandDetection _handDetection;
    private Transform _cameraTransform;
    private FocusHighlighter _currentFocus;
    private GesturesManager _gesturesManager;


    void Start()
    {
        _tileParent = GetComponentInParent<WorldGridTile>();
        _handDetection = FindObjectOfType<HandDetection>();
        _cameraTransform = Camera.main.transform;
        _gesturesManager = Camera.main.GetComponent<GesturesManager>();
    }
    
    //void Update()
    //{
    //    MoveBuilding();
    //}



    void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
    {
        if (_gesturesManager.IsNavigating) return;

        InputManager.Instance.PushModalInputHandler(gameObject);
                
        _startPosition = _handDetection.GetHandPos();
        _startPosition += _cameraTransform.forward * offset;

        _startRotation = transform.rotation.y;
        transform.position = _startPosition;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        
        eventData.Use();
    }

    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (!InputManager.Instance.CheckModalInputStack(gameObject) || _gesturesManager.IsNavigating) return;

        transform.position = _startPosition + eventData.CumulativeDelta;
        Quaternion rot = Quaternion.LookRotation(_cameraTransform.forward);
        rot.z = rot.x = 0;
        rot.y += _startRotation;
        transform.rotation = rot;

        MoveBuilding();

        eventData.Use();

    }

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
        SwapBuilding();
        eventData.Use();
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        SwapBuilding();
        eventData.Use();
    }

    void SwapBuilding()
    {
        if (_gesturesManager.IsNavigating) return;

        InputManager.Instance.PopModalInputHandler();

        _tileParent.SnapRotation(transform);
        _tileParent.AttemptBuildingSwap(transform.position);

        transform.localPosition = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("Hologram");
    }

    void MoveBuilding()
    {
       if (_gesturesManager.IsNavigating || !InputManager.Instance.CheckModalInputStack(gameObject)) return;
        
        LayerMask layerMask = LayerMask.NameToLayer("Hologram");
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, layerMask))
        {
            FocusHighlighter focus = hit.transform.GetComponent<FocusHighlighter>();

            if (focus)
            {
                if (focus != _currentFocus)
                {
                    if (_currentFocus)
                        _currentFocus.ResetColour();

                    focus.HighlightObject();
                    _currentFocus = focus;
                }
            }
        }
    }

}