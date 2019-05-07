using Infrastructure;
using Infrastructure.Grid;
using Infrastructure.Grid.Entities;
using Infrastructure.Grid.Entities.Buildings;
using Infrastructure.Residents;
using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SessionCreator : MonoBehaviour
{

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

    public void StartNewGame(int timePeriod)
    {
        Game.SetSession(new Session((TimePeriod)timePeriod));
        //We subscribe to know when the session is ready.
        //We must do this as we make a new tick manager and this is not in this thread.
        Game.CurrentSession.OnSessionReady += _sessionReadyDel;
    }

    public void StartGame(SaveData data)
    {
        DeleteGame();
        gridInfo = data.gridInfo ?? null;
        //Create a new session but use the saved data.
        Game.SetSession(new Session(data.Settings, data.Name, data.CreationDate));
        //We subscribe to know when the session is ready.
        //We must do this as we make a new tick manager and this is not in this thread.
        Game.CurrentSession.OnSessionReady += _sessionReadyDel;
    }

    private void DeleteGame()
    {
        if (Game.CurrentSession != null)
        {
            Game.CurrentSession.City.DeleteCity();
            Game.CurrentSession.DestroySession();
        }
    }

    public void Update()
    {
        //We know the session is ready, now we can make our grid.
        if (_sessionReady)
        {
            _sessionReady = false;
            Game.CurrentSession.OnSessionReady -= _sessionReadyDel;

            if (gridInfo.Count > 0) LoadDataFromSave();
            else CreateDefaultGrid();

            Game.CurrentSession.TickManager.Start();
        }
    }

    private void CreateDefaultGrid()
    {
        Game.CurrentSession.City.CreateGrid(width, height, (Camera.main.transform.forward * 1.5f) - new Vector3(0, 0.5f));
    }

    /// <summary>
    /// Load grid data and restore their contents.
    /// </summary>
    public void LoadDataFromSave()
    {
        if (gridInfo.Count == 0) return;

        List<BuildingMap> buildingMap = BuildingLibrary.GetBuildingsForCategory(BuildingCategory.All);

        //Loop and restore each grid entity for each grid.
        for (int i = 0; i < gridInfo.Count; i++)
        {
            Game.CurrentSession.City.CreateGrid(gridInfo[i].width, gridInfo[i].height, gridInfo[i].worldPos);
            GridSystem grid = Game.CurrentSession.City.GetGrid(i);

            for (int j = 0; j < gridInfo[i].gridEntities.Count; j++)
            {
                int x = gridInfo[i].gridEntities[j].x;
                int y = gridInfo[i].gridEntities[j].y;
                int index = gridInfo[i].gridEntities[j].z;

                //Manually handle the road typing.
                Type entityType;
                if (index == -1) entityType = typeof(Road);
                else entityType = buildingMap[index].BuildingType;

                TileEntity tileEnt = Activator.CreateInstance(entityType) as TileEntity;

                grid.AddTileEntityToTile(x, y, tileEnt);
            }

            //Restore residents AFTER grid entities have been restored.
            foreach (ResidentData resInfo in gridInfo[i].residentData)
            {
                Resident newResident = null; //Silly vs
                try
                {
                    newResident = new Resident(resInfo);
                    Game.CurrentSession.City.Residents.Add(newResident);
                    Residential res = (Residential)Game.CurrentSession.City.GetGrid(resInfo.HomeGridID).GetTile(resInfo.HomePosition)?.Entity;
                    if (res != null) res.AddResident(newResident);
                    else
                    {
                        Game.CurrentSession.City.ProcessHomelessResident(newResident);
                    }

                    foreach(Job j in ((Commercial)Game.CurrentSession.City.GetGrid(resInfo.JobGridID).GetTile(resInfo.JobPosition).Entity).Jobs)
                    {
                        if (!j.Taken)
                        {
                            newResident.SetJob(j);
                            break;
                        }
                    }
                    if(newResident.Job == null)
                    {
                        //Coult not give this resident a job? leave it unemployed.
                        Game.CurrentSession.City.UnemployedResidents.Add(newResident);
                    }
                }
                catch (Exception)
                {
                    //Abandon loading this resident.
                    //Try to remove it if we added it earlier.
                    if (newResident != null) {
                        if (Game.CurrentSession.City.Residents.Contains(newResident)) Game.CurrentSession.City.Residents.Remove(newResident);
                    }
                    //Increase demand to offset the lost resident.
                    Game.CurrentSession.Settings.ResidentialDemand++;
                }
            }

        }
    }

}
