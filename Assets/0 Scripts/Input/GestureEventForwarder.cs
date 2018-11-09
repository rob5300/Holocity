using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine;

public class GestureEventForwarder : MonoBehaviour, IManipulationHandler
{
    //These events are are what are subbed to by the WorldGridTile to recieve these events.
    //This may be a bad idea performance wise and may change later, though c# events are quite fast.
    public event Action<ManipulationEventData> E_ManipulationStarted;
    public event Action<ManipulationEventData> E_ManipulationUpdated;
    public event Action<ManipulationEventData> E_ManipulationCompleted;
    public event Action<ManipulationEventData> E_ManipulationCanceled;

    //Event catcher methods, these call the event if they have something subbed.

    void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
    {
        E_ManipulationStarted?.Invoke(eventData);
    }

    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
        E_ManipulationUpdated?.Invoke(eventData);
    }

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
        E_ManipulationCompleted?.Invoke(eventData);
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        E_ManipulationCanceled?.Invoke(eventData);
    }
}
