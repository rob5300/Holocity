using Infrastructure.Grid;
using Infrastructure.Grid.Entities;
using Infrastructure.Grid.Entities.Buildings;
using Infrastructure.Residents;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SessionCreator : MonoBehaviour {

    [Header("Grid settings")]
    public int width;
    public int height;

    private bool _sessionReady = false;
    private List<GridInfo> gridInfo = new List<GridInfo>();

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

    public void StartGame(List<GridInfo> info)
    {
        gridInfo = info ?? null;
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

            if (gridInfo.Count > 0) LoadGrids();
            else CreateDefaultGrid();


            Game.CurrentSession.OnSessionReady -= _sessionReadyDel;
        }
    }

    private void CreateDefaultGrid()
    {
        Game.CurrentSession.City.CreateGrid(width, height, (Camera.main.transform.forward * 1.5f) - new Vector3(0, 0.5f));

        GridSystem grid = Game.CurrentSession.City.GetGrid(0);

        grid.AddTileEntityToTile(0, 0, new PowerPlant());
        grid.AddTileEntityToTile(0, 1, new House());
        grid.AddTileEntityToTile(1, 0, new Flat1());
        grid.AddTileEntityToTile(2, 0, new Flat2());
        grid.AddTileEntityToTile(1, 1, new Future_House());   
    }

    public void LoadGrids()
    {
        if (gridInfo.Count == 0) return;

        List<BuildingMap> buildingMap = BuildingLibrary.GetListForTimePeriod(BuildingCategory.All);

        for (int i =0; i < gridInfo.Count; i++)
        {
            Game.CurrentSession.City.CreateGrid(gridInfo[i].width, gridInfo[i].height, gridInfo[i].worldPos);
            GridSystem grid = Game.CurrentSession.City.GetGrid(i);

            for(int j = 0; j < gridInfo[i].gridEntities.Count; j++)
            {
                int x = gridInfo[i].gridEntities[j].x;
                int y = gridInfo[i].gridEntities[j].y;
                int index = gridInfo[i].gridEntities[j].z;
                
                TileEntity tileEnt = Activator.CreateInstance(buildingMap[index].BuildingType) as TileEntity;
                
                grid.AddTileEntityToTile(x,y,tileEnt);
            }

            foreach (ResidentInfo resInfo in gridInfo[i].residentInfo)
            {

                //Residential home = grid.GetTile(resInfo.HomePosition).Entity.;
                //Resident resident = new Resident();

                //grid.Residents.Add(resident);
                //home.SetResident(resident);
            }

        }
    }

}
