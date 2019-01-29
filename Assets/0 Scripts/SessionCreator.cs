using Infrastructure.Grid;
using Infrastructure.Grid.Entities;
using Infrastructure.Grid.Entities.Buildings;
using UnityEngine;

public class SessionCreator : MonoBehaviour {

    [Header("Grid settings")]
    public int width;
    public int height;

	void Start () {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif
        Game.SetSession(new Session());
        Debug.Log("New session created and set: " + Game.CurrentSession.Name);

        //Give the player some money
        Game.CurrentSession.AddFunds(1000);

        Debug.Log(BuildingLibrary.ModernBuildings.Count);
    }

    public void CreateDefaultGrid()
    {
        Game.CurrentSession.City.CreateGrid(width, height, transform.position);

        GridSystem grid = Game.CurrentSession.City.GetGrid(0);

        grid.AddTileEntityToTile(0, 0, new PowerPlant());
        grid.AddTileEntityToTile(0, 1, new House());
        grid.AddTileEntityToTile(0, 2, new House());
        grid.AddTileEntityToTile(0, 3, new House());
        grid.AddTileEntityToTile(0, 6, new WaterPlant());
        grid.AddTileEntityToTile(1, 0, new Road());
        grid.AddTileEntityToTile(2, 0, new Road());
        grid.AddTileEntityToTile(3, 0, new Road());
    }

}
