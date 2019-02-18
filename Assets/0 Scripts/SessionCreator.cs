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
        Game.CurrentSession.AddFunds(90123);

        Debug.Log(BuildingLibrary.ModernBuildings.Count);
    }

    public void CreateDefaultGrid()
    {
        Game.CurrentSession.City.CreateGrid(width, height, transform.position);

        GridSystem grid = Game.CurrentSession.City.GetGrid(0);

        grid.AddTileEntityToTile(0, 0, new PowerPlant());
        grid.AddTileEntityToTile(0, 1, new House());
    }

}
