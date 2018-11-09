using UnityEngine;
using System;

[SelectionBase]
public class WorldGridTile : MonoBehaviour {

    public WorldGrid ParentGrid;
    public Vector2Int Position;
    public GameObject TileBorder;
    public GameObject Model;

    public event Action OnInteract;
    public event Action OnHoverBegin;
    public event Action OnHoverEnd;
    public event Action OnMoveBegin;
    public event Action<WorldGridTile> OnMoveEnd;
    public event Action OnMoveFail;
    public event Action OnTileDestroy;

    public void OnDestroy()
    {
        OnTileDestroy?.Invoke();
    }

}
