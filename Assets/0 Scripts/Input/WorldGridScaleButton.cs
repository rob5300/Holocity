using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.XR.WSA;

public class WorldGridScaleButton : MonoBehaviour, INavigationHandler
{
    public WorldGrid GridParent;
    private Vector3 _startScale;


    float ScaleSpeed = 2.0f;

    public void OnNavigationCanceled(NavigationEventData eventData)
    {

        InputManager.Instance.PopModalInputHandler();

        if (!GridParent.transform.GetComponent<WorldAnchor>()) GridParent.transform.gameObject.AddComponent<WorldAnchor>();
        eventData.Use();
    }

    public void OnNavigationCompleted(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        if (!GridParent.transform.GetComponent<WorldAnchor>()) GridParent.transform.gameObject.AddComponent<WorldAnchor>();
        eventData.Use();
    }

    public void OnNavigationStarted(NavigationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);

        if (GridParent.transform.GetComponent<WorldAnchor>()) DestroyImmediate(GridParent.transform.GetComponent<WorldAnchor>());

        _startScale = GridParent.transform.localScale;

        eventData.Use();
    }

    public void OnNavigationUpdated(NavigationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);

        Vector3 scale = _startScale * (Mathf.Clamp(1 + eventData.NormalizedOffset.x, 0.2f, 3.0f));

        if (scale.x < 0.5f) scale.x = scale.y = scale.z = 0.8f;
        if (scale.x > 3.0f) scale.x = scale.y = scale.z = 3.0f;

        GridParent.transform.localScale = scale;
        eventData.Use();
    }

}
