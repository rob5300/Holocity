using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class PlayerGestureInput : MonoBehaviour, IInputClickHandler
{
    private GazeManager gaze;

    public void Start()
    {
        //Get an instance to the GazeManager.
        gaze = GazeManager.Instance;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if(gaze != null)
        {
            IPlayerClick hitOb = gaze.gameObject.GetComponent<IPlayerClick>();
            if(hitOb != null)
            {
                //Here we can do some feedback if this is allowed/successful or not.
                hitOb.OnPlayerClick(this, new PlayerClickEventData());
            }
        }
    }
}
