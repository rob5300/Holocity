using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Buttons;

public class TapGesture : MonoBehaviour, IInputClickHandler {
    
    float timeofuse = -999;


  
    public void OnInputClicked(InputClickedEventData eventData)
    {
        if(timeofuse + 0.5f > Time.time) return;

        timeofuse = Time.time;

        eventData.Use();

        if (UIManager.Instance.roadTool.active)
        {
            UIManager.Instance.roadTool.TilePressed();
        }
        else if (UIManager.Instance.buildingTool.active)
        {
            UIManager.Instance.buildingTool.TilePressed();
        }
        else if (GazeManager.Instance.HitObject && GazeManager.Instance.HitObject.GetComponent<CompoundButton>())
        {
            AudioManager.Instance.UISound(true);
        }
        else if (GazeManager.Instance.HitObject && GazeManager.Instance.HitObject.GetComponentInParent<WorldGridTile>())
        {
            WorldGridTile tile = GazeManager.Instance.HitObject.transform.parent.GetComponent<WorldGridTile>();
            UIManager.Instance.TilePressed(tile);
            AudioManager.Instance.SelectSound(true);
        }
        else if (!GazeManager.Instance.HitObject && UIManager.Instance.menuState != UIManager.MenuState.Off)
        {
            UIManager.Instance.SwitchState(UIManager.MenuState.Off);
            AudioManager.Instance.SelectSound(false);
            //turn off UI if player clicks away.
        }
        

    }

}
