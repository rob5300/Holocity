using CityResources;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour
{
    public float UpdateDelay = 0.25f;
    public ResourceDataInfo[] ResourceDataObjects;

    private Electricity _electricity;
    private float _lastUpdateTime = -999;

    public void Start()
    {
        _electricity = Game.CurrentSession.City.GetResource<Electricity>();
    }

    public void Update()
    {
        if(_lastUpdateTime + UpdateDelay < Time.time)
        {
            ResourceDataObjects[0].ValueText.text = _electricity.Value.ToString();
            ResourceDataObjects[0].InputText.text = _electricity.AddedLastTick.ToString();
            ResourceDataObjects[0].OutputText.text = _electricity.RecievedLastTick.ToString();
            _lastUpdateTime = Time.time;
        }
    }
}

[System.Serializable]
public class ResourceDataInfo
{
    public Text ValueText;
    public Text InputText;
    public Text OutputText;
}

