using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : MonoBehaviour {

    [NonSerialized]
    public WorldGridTile[][] GridTiles;
#if UNITY_EDITOR
    [Header("Debug Options")]
    public int width;
    public int height;
#endif


    [ContextMenu("Initialize Grid")]
    private void initDebug()
    {
        Initialize(width, height);
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
    }

    private WorldGridTile CreateWorldGridTile(Vector2Int position)
    {
        GameObject newTile = new GameObject();
        newTile.name = string.Format("WorldTile({0},{1})", position.x, position.y);
        newTile.transform.SetParent(transform);
        newTile.transform.localPosition = new Vector3(position.x, 0, position.y);
        WorldGridTile tile = newTile.AddComponent<WorldGridTile>();
        newTile.AddComponent<MeshRenderer>();
        Instantiate(Game.CurrentSession.Cache.TileBorder, newTile.transform);
        tile.Position = position;
        return tile;
    }
}
