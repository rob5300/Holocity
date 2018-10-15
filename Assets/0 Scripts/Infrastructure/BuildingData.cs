using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Create Building Data Asset")]
public class BuildingData : ScriptableObject {

    public string Name;
    public string Description;
    public Sprite Icon;

}
