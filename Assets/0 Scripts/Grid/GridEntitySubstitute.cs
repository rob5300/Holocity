using Grid;
using UnityEngine;

public class GridEntitySubstitute : MonoBehaviour, IPlayerClick {

    public GridEntity Entity;

    public bool OnPlayerClick(object sender, PlayerClickEventData eventData)
    {
        Debug.Log("I was clicked by the player");
        return false;
    }

}
