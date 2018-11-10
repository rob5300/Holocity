using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGridMoveButton : MonoBehaviour, INavigationHandler /*,IManipulationHandler */
{
    public WorldGrid GridParent;

    public void OnNavigationCanceled(NavigationEventData eventData)
    {
        transform.SetParent(GridParent.GridContainer.transform);
    }

    public void OnNavigationCompleted(NavigationEventData eventData)
    {
        transform.SetParent(GridParent.GridContainer.transform);
    }

    public void OnNavigationStarted(NavigationEventData eventData)
    {
        transform.SetParent(null);
    }

    public void OnNavigationUpdated(NavigationEventData eventData)
    {
        transform.position += eventData.NormalizedOffset * Time.deltaTime;
        GridParent.GridContainer.transform.position += eventData.NormalizedOffset * Time.deltaTime;
    }
}
