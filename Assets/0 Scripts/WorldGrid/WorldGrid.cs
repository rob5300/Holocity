using Infrastructure.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : MonoBehaviour {

    public int Width;
    public int Height;
    public int Id;
    public GridSystem GridSystem;
    public GameObject GridContainer { get; private set; }
    public GameObject ResourceUI;

    public WorldGridTaskManager TaskManager;

    /// <summary>
    /// How wide a tile will be. Is used to scale position movements
    /// </summary>
    public float TileScale = 0.15f;

    /// <summary>
    /// The scale to apply to the container to reduce its size.
    /// </summary>
    public Vector3 ContainerScaleFactor = new Vector3(0.5f, 0.5f, 0.5f);

    [NonSerialized]
    public WorldGridTile[][] GridTiles;

    public void Awake()
    {
        GridContainer = new GameObject("Grid Container");
        GridContainer.transform.SetParent(transform);
        GridContainer.transform.localPosition = Vector3.zero;

        TaskManager = gameObject.AddComponent<WorldGridTaskManager>();
    }

	public void Initialize(int id, int width, int height, GridSystem gridSystem)
    {
        GridSystem = gridSystem;
        GridTiles = new WorldGridTile[width][];
        for(int w=0; w < width; w++)
        {
            GridTiles[w] = new WorldGridTile[height];
            for(int h=0; h < height; h++)
            {
                GridTiles[w][h] = CreateWorldGridTile(new Vector2Int(w, h));
            }
        }
        Width = width;
        Height = height;
        CenterGridTiles();
        AddMoveButton(new Vector3(GridTiles[0][0].transform.localPosition.x - 0.15f, GridTiles[0][0].transform.localPosition.y, GridTiles[0][0].transform.localPosition.z - 0.15f));

        //Add in the resources UI.
        AddResourcesUI();

        //Attempt to scale the grid container
        GridContainer.transform.localScale = ContainerScaleFactor;

        RotateToFaceUser();
    }

    public WorldGridTile GetTile(Vector2Int position)
    {
        return GetTile(position.x, position.y);
    }

    public WorldGridTile GetTile(int x, int y)
    {
        if (x > Width || y > Height) return null;
        return GridTiles[x][y];
    }

    private WorldGridTile CreateWorldGridTile(Vector2Int tileposition)
    {
        GameObject newTileGameObject = new GameObject();
        newTileGameObject.name = string.Format("WorldTile({0},{1})", tileposition.x, tileposition.y);
        newTileGameObject.transform.SetParent(GridContainer.transform);
        newTileGameObject.transform.localPosition = new Vector3(tileposition.x * TileScale, 0, tileposition.y * TileScale);
        //Add WorldGridTile component and set up.
        WorldGridTile tile = newTileGameObject.AddComponent<WorldGridTile>();
        tile.Initialize(tileposition, this);

        return tile;
    }

    /// <summary>
    /// Swap the positions of these two tiles. Simply a data swap and visual only.
    /// </summary>
    /// <param name="tileA"></param>
    /// <param name="tileB"></param>
    public void SwapGridTiles(Vector2Int tileA, Vector2Int tileB)
    {
        //Get Tiles.
        WorldGridTile a = GetTile(tileA);
        WorldGridTile b = GetTile(tileB);
        //Store the old a positions.
        Vector3 oldAPos = a.transform.position;
        Vector2Int oldATilePos = a.Position;
        //Change a to b's data.
        a.Position = b.Position;
        a.transform.position = b.transform.position;
        GridTiles[b.Position.x][b.Position.y] = a;
        //Change b's data to a's.
        b.Position = oldATilePos;
        b.transform.position = oldAPos;
        GridTiles[oldATilePos.x][oldATilePos.y] = b;
    }

    private void AddMoveButton(Vector3 position)
    {
        GameObject moveButton = Instantiate(Game.CurrentSession.Cache.MoveButton, GridContainer.transform);
        moveButton.transform.localPosition = position;
        moveButton.GetComponent<WorldGridMoveButton>().GridParent = this;
    }

    [ContextMenu("Center Grid Tiles")]
    private void CenterGridTiles()
    {
        //Calculate the offset to apply to positions to centre the tiles.
        Vector3 offset = new Vector3((Width / 2 * TileScale) - TileScale / 2, 0, (Height / 2 * TileScale) - TileScale / 2);
        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
                GridTiles[w][h].transform.localPosition = GridTiles[w][h].transform.localPosition - offset;
            }
        }
    }

    /// <summary>
    /// Rotates the grid container to face the user when placed.
    /// </summary>
    private void RotateToFaceUser()
    {
        Vector3 target = Camera.main.transform.position - GridContainer.transform.position;
        target.y = 0;
        GridContainer.transform.rotation = Quaternion.LookRotation(target);
        //We rotate 180 on Y otherwise we are facing away from the player.
        GridContainer.transform.Rotate(0, 180, 0);
    }

    private void AddResourcesUI()
    {
        ResourceUI = Instantiate(Game.CurrentSession.Cache.ResourceUI, GridContainer.transform);
        ResourceUI.transform.localPosition = new Vector3(0, 0.3f, 0);
    }
}
