using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;

public class ScrollBarNavigation : MonoBehaviour, INavigationHandler {

    public RectTransform buttonPanel;
    public Scrollbar scrollBar;
    private int buttonCount;
    private float minPos;
    private Vector3 newPos = new Vector3(0, 0.11f,-0.0078f);

    private void Awake()
    {
        UIManager.Instance.BuildingsGenerated += UpdateCount;
    }

    void UpdateCount(int count)
    {
        buttonCount = count;
        minPos = (Mathf.CeilToInt(buttonCount / 2f) - 3f) * -0.13f;
    }

    public void UpdatePanelPosition()
    {
        if(buttonCount <= 6)
        {
            scrollBar.value = 0;
            newPos.x = 0;
        }
        else
        {
            newPos.x = scrollBar.value * minPos;
        }

        buttonPanel.localPosition = newPos;
    }

    void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
    {
       InputManager.Instance.PushModalInputHandler(gameObject);
       eventData.Use();
    }

    void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
    {
        scrollBar.value += eventData.NormalizedOffset.x /3f;
        eventData.Use();
    }

    void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        eventData.Use();
    }

    void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        eventData.Use();
    }
}