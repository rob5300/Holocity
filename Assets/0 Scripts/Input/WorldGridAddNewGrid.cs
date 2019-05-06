using HoloToolkit.Unity.InputModule;
using Infrastructure.Grid;
using UnityEngine;

public class WorldGridAddNewGrid : MonoBehaviour, IManipulationHandler
{
    public WorldGrid GridParent;

    private Vector3 _startPosition;
    private Vector3 _gridStartPosition;
    private GridSystem _grid;
    private WorldGrid _gridWorld;

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();

        eventData.Use();
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        _grid = null;
        _gridWorld = null;
        eventData.Use();
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);

        _startPosition = transform.position;
        eventData.Use();

        //Make a new grid
        _grid = Game.CurrentSession.City.CreateGrid(10, 10, GridParent.transform.position + new Vector3(-1.5f,0,0));
        _gridWorld = _grid.WorldGrid;
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        _gridWorld.transform.position += _startPosition + eventData.CumulativeDelta;
        eventData.Use();
    }

}
