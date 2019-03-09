using Infrastructure.Grid;
using Infrastructure.Grid.Entities;
using Infrastructure.Grid.Entities.Buildings;
using UnityEngine;

public class SessionCreator : MonoBehaviour {

    [Header("Grid settings")]
    public int width;
    public int height;

	public void StartNewGame () {
        Game.SetSession(new Session());
    }

    public void CreateDefaultGrid()
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
