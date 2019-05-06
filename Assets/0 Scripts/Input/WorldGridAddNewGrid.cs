using HoloToolkit.Unity.InputModule;
using Infrastructure.Grid;
using UnityEngine;
using UnityEngine.XR.WSA;

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

        if (!_gridWorld.transform.GetComponent<WorldAnchor>()) _gridWorld.transform.gameObject.AddComponent<WorldAnchor>();

        _grid = null;
        _gridWorld = null;
        
        eventData.Use();
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();


        if (!_gridWorld.transform.GetComponent<WorldAnchor>()) _gridWorld.transform.gameObject.AddComponent<WorldAnchor>();

        _grid = null;
        _gridWorld = null;


        eventData.Use();
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);

        _startPosition = transform.position;
        

        //Make a new grid
        _grid = Game.CurrentSession.City.CreateGrid(10, 10, GridParent.transform.position + new Vector3(-1.5f,0,0));
        _gridWorld = _grid.WorldGrid;

        if (_gridWorld.transform.GetComponent<WorldAnchor>()) DestroyImmediate(_gridWorld.transform.GetComponent<WorldAnchor>());

        eventData.Use();
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        if(_gridWorld)
        _gridWorld.transform.position = _startPosition + eventData.CumulativeDelta;
        eventData.Use();
    }

}
