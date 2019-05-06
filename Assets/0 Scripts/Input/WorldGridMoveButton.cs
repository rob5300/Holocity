using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.XR.WSA;

public class WorldGridMoveButton : MonoBehaviour ,IManipulationHandler
{
    public WorldGrid GridParent;

    private Vector3 _startPosition;
    private Vector3 _gridStartPosition;

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        //transform.SetParent(GridParent.GridContainer.transform);
        if (!GridParent.transform.GetComponent<WorldAnchor>()) GridParent.transform.gameObject.AddComponent<WorldAnchor>();
        eventData.Use();

    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        //transform.SetParent(GridParent.GridContainer.transform);
        if (!GridParent.transform.GetComponent<WorldAnchor>()) GridParent.transform.gameObject.AddComponent<WorldAnchor>();
        eventData.Use();

    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
        if (GridParent.transform.GetComponent<WorldAnchor>()) DestroyImmediate(GridParent.transform.GetComponent<WorldAnchor>());

        _startPosition = GridParent.transform.position;
        //_gridStartPosition = GridParent.GridContainer.transform.position;
        eventData.Use();

    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        //transform.position = _startPosition + eventData.CumulativeDelta;

        GridParent.transform.position = _startPosition + eventData.CumulativeDelta;

       // GridParent.GridContainer.transform.position = _gridStartPosition + eventData.CumulativeDelta;
        eventData.Use();

    }

}
