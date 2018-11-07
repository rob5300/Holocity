using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : MonoBehaviour {

    public int Width;
    public int Height;
    
    /// <summary>
    /// How wide a tile will be. Is used to scale position movements
    /// </summary>
    public float Scale = 0.15f;

    [NonSerialized]
    public WorldGridTile[][] GridTiles;

#if UNITY_EDITOR
    [Header("Debug Options")]
    public int widthdebug;
    public int heightdebug;
#endif

    private GameObject _gridContainer;

    public void Awake()
    {
        _gridContainer = new GameObject("Grid Container");
        _gridContainer.transform.SetParent(transform);
    }

    [ContextMenu("Initialize Grid")]
    private void initDebug()
    {
        Initialize(widthdebug, heightdebug);
    }

	public void Initialize(int width, int height)
    {
        GridTiles = new WorldGridTile[width][];
        for(int w=0; w < width; w++)
        {
            GridTiles[w] = new WorldGridTile[height];
            for(int h=0; h < height; h++)
            {
                GridTiles[w][h] = CreateWorldGridTile(new Vector2Int(w, h));
            }
        }
        //Wont apply scale like this as it causes wierd issues.
        //_gridContainer.transform.localScale = new Vector3(Scale, Scale, Scale);
        Width = width;
        Height = height;
        //CenterGridTiles();
    }

    public WorldGridTile GetTile(int x, int y)
    {
        if (x > Width || y > Height) return null;
        return GridTiles[x][y];
    }

    private WorldGridTile CreateWorldGridTile(Vector2Int position)
    {
        GameObject newTileGameObject = new GameObject();
        newTileGameObject.name = string.Format("WorldTile({0},{1})", position.x, position.y);
        newTileGameObject.transform.SetParent(_gridContainer.transform);
        newTileGameObject.transform.localPosition = new Vector3(position.x * Scale, 0, position.y * Scale);
        //Add WorldGridTile component and set up.
        WorldGridTile tile = newTileGameObject.AddComponent<WorldGridTile>();
        tile.ParentGrid = this;
        tile.TileBorder = Instantiate(Game.CurrentSession.Cache.TileBorder, newTileGameObject.transform);
        tile.Position = position;
        return tile;
    }

    private void CenterGridTiles()
    {
        Vector3 offset = new Vector3((Width / 2) - (Scale / 2), 0, (Height / 2) - (Scale / 2));
        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
                GridTiles[w][h].transform.position = GridTiles[w][h].transform.position - offset;
            }
        }
    }
}
