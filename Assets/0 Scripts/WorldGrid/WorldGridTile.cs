using UnityEngine;

[SelectionBase]
public class WorldGridTile : MonoBehaviour {

    public WorldGrid ParentGrid;
    public Vector2Int Position;
    public GameObject TileBorder;
    public GameObject Model;

    public event System.Action OnInteract;

}
