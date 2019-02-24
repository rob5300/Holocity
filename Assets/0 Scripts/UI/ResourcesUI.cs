using CityResources;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesUI : MonoBehaviour
{
    public float UpdateDelay = 0.25f;
    public ResourceDataInfo[] ResourceDataObjects;
    public TextMeshPro GridHappinessText;
    public TextMeshPro FundsText;
    public Toggle move;
    public Toggle rotate;
    public Color ToggleColor;

    private Electricity _electricity;
    private Water _water;
    private float _lastUpdateTime = -999;
    [NonSerialized]
    public WorldGrid WorldGrid;

    //Rotate resource towards player
    private Camera camera;

    public void Start()
    {
        UIManager.Instance.StateChanged += ToggleUI;
        _electricity = Game.CurrentSession.City.GetResource<Electricity>();
        _water = Game.CurrentSession.City.GetResource<Water>();
        camera = GameObject.Find("MixedRealityCamera").GetComponent<Camera>();
        SwitchMode(GesturesManager.Instance.IsNavigating);
    }

    public void Update()
    {
        transform.rotation = Quaternion.LookRotation(camera.transform.forward);
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

    public void SwitchMode(bool navigate)
    {
        if (navigate)
        {
          
            rotate.image.color = ToggleColor;
            move.image.color = Color.white;
        }
        else
        {
            rotate.image.color = Color.white;
            move.image.color = ToggleColor;
        }
        GesturesManager.Instance.SwitchMode(navigate);
    }

    void ToggleUI(bool check)
    {
        gameObject.SetActive(check);
    }
}


[System.Serializable]
public class ResourceDataInfo
{
    public Text ValueText;
    public Text InputText;
    public Text OutputText;
}

