using Infrastructure.Grid;
using Infrastructure.Grid.Entities.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Infrastructure.Tick.TickManager;

public class SessionCreator : MonoBehaviour {

    [Header("Grid settings")]
    public int width;
    public int height;

	void Start () {
        Game.SetSession(new Session());
        Debug.Log("New session created and set: " + Game.CurrentSession.Name);
	}

    public void CreateDefaultGrid()
    {
        Game.CurrentSession.City.CreateGrid(width, height, transform.position);

        GridSystem grid = Game.CurrentSession.City.GetGrid(0);

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                grid.AddBuildingToTile(x, y, new PowerPlant());
            }
        }
    }

}
