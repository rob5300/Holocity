using Infrastructure.Grid;
using Infrastructure.Grid.Entities.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionCreator : MonoBehaviour {

    [Header("Grid settings")]
    public int width;
    public int height;
    public bool CreateDefaultGrid = true;

	void Start () {
        Game.SetSession(new Session());
        Debug.Log("New session created and set: " + Game.CurrentSession.Name);

        if (!CreateDefaultGrid) return;

        Game.CurrentSession.City.CreateGrid(width, height, Vector3.zero);

        Game.CurrentSession.City.GetGrid(0).AddBuildingToTile(1, 1, new House());
        Game.CurrentSession.City.GetGrid(0).AddBuildingToTile(1, 5, new House());
	}
	
    [ContextMenu("Swap test")]
    public void SwapTest()
    {
        Game.CurrentSession.City.GetGrid(0).SwapTileEntities(new Vector2Int(1, 1), new Vector2Int(5, 5));
    }
}
