using CityResources;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour
{
    public float UpdateDelay = 0.25f;
    public ResourceDataInfo[] ResourceDataObjects;
    public Text GridHappinessText;
    public Text FundsText;

    private Electricity _electricity;
    private Water _water;
    private float _lastUpdateTime = -999;
    [NonSerialized]
    public WorldGrid WorldGrid;

    public void Start()
    {
        _electricity = Game.CurrentSession.City.GetResource<Electricity>();
        _water = Game.CurrentSession.City.GetResource<Water>();
    }

    public void Update()
    {
        //Update the resources ui for city resources.
        if(_lastUpdateTime + UpdateDelay < Time.time)
        {
            //Electricity
            ResourceDataObjects[0].ValueText.text = _electricity.Value.ToString();
            ResourceDataObjects[0].InputText.text = _electricity.AddedLastTick.ToString();
            ResourceDataObjects[0].OutputText.text = _electricity.RecievedLastTick.ToString();
            //Water
            ResourceDataObjects[1].ValueText.text = _water.Value.ToString();
            ResourceDataObjects[1].InputText.text = _water.AddedLastTick.ToString();
            ResourceDataObjects[1].OutputText.text = _water.RecievedLastTick.ToString();
            _lastUpdateTime = Time.time;
            //Update Money
            FundsText.text = Game.CurrentSession.Funds.ToString();
        }
        //Update the happiness value in the UI
        float happiness = WorldGrid.GridSystem.AverageResidentHappiness;
        GridHappinessText.text = happiness.ToString();

    }
}

[System.Serializable]
public class ResourceDataInfo
{
    public Text ValueText;
    public Text InputText;
    public Text OutputText;
}

