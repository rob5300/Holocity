using HoloToolkit.Unity.Buttons;
using TMPro;
using UnityEngine;

public class BuildingButton : MonoBehaviour {

    public int index = 0;

    public TextMeshPro costText;
    public TextMeshPro nameText;
    public GameObject modelParent;
    Material[] mats;

    public void ScaleModel(float iconSize)
    {
        float height = modelParent.GetComponentInChildren<MeshRenderer>().bounds.size.x;
        float scale = iconSize / height;
        modelParent.transform.localScale *= scale;

        mats = modelParent.GetComponentInChildren<MeshRenderer>().materials;
        
        foreach(Material mat in mats)
        {
         //   mat.renderQueue = 5000;
        }
    }
    
}
