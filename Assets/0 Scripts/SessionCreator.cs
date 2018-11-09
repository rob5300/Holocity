using Infrastructure.Grid;
using Infrastructure.Grid.Entities.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionCreator : MonoBehaviour {

    [Header("Grid settings")]
    public int width;
    public int height;

	void Start () {
        Game.SetSession(new Session());
        Debug.Log("New session created and set: " + Game.CurrentSession.Name);
	}
	
    [ContextMenu("Swap test")]
    public void SwapTest()
    {
        Game.CurrentSession.City.GetGrid(0).SwapTileEntities(new Vector2Int(1, 1), new Vector2Int(5, 5));
    }
}
