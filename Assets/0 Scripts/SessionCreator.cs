using Infrastructure.Grid;
using Infrastructure.Grid.Entities;
using Infrastructure.Grid.Entities.Buildings;
using System;
using UnityEngine;

public class SessionCreator : MonoBehaviour {

    [Header("Grid settings")]
    public int width;
    public int height;

    private bool _sessionReady = false;
    private Action<Session> _sessionReadyDel;

    public void Awake()
    {
        //A thread safe way for us to know when the session is ready.
        _sessionReadyDel = (sess) => { _sessionReady = true; };
    }

    public void StartNewGame () {
        Game.SetSession(new Session());
        //We subscribe to know when the session is ready.
        //We must do this as we make a new tick manager and this is not in this thread.
        Game.CurrentSession.OnSessionReady += _sessionReadyDel;
    }

    public void Update()
    {
        //We know the session is ready, now we can make our grid.
        if (_sessionReady)
        {
            _sessionReady = false;
            CreateDefaultGrid();
            Game.CurrentSession.OnSessionReady -= _sessionReadyDel;
        }
    }

    private void CreateDefaultGrid()
    {
        Game.CurrentSession.City.CreateGrid(width, height, (Camera.main.transform.forward * 1.5f) - new Vector3(0, 0.3f));

        GridSystem grid = Game.CurrentSession.City.GetGrid(0);

        grid.AddTileEntityToTile(0, 0, new PowerPlant());
        grid.AddTileEntityToTile(0, 1, new House());
        grid.AddTileEntityToTile(1, 0, new Flat1());
        grid.AddTileEntityToTile(2, 0, new Flat2());
        grid.AddTileEntityToTile(1, 1, new Future_House());   
    }

}
