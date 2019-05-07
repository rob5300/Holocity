using CityResources;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesUI : MonoBehaviour
{
    public float UpdateDelay = 0.25f;
    public ResourceDataInfo[] ResourceDataObjects;
    public TextMeshPro GridHappinessText;
    public TextMeshPro FundsText;
    public TextMeshPro ResidentDemandText;
    public TextMeshPro ResidentTotal;
    public TextMeshPro DistrictName;
    public Toggle move;
    public Toggle rotate;
    public Color ToggleColor;

    public GameObject bolt;
    public GameObject fire;


    private float _lastUpdateTime = -999;
    [NonSerialized]
    public WorldGrid WorldGrid;

    private Camera lookCamera;
    private List<ResourceData> waterResources;
    private List<ResourceData> electricityResources;

    public void Awake()
    {
        DistrictName.text = GenerateDistrictName();

        if (Game.CurrentSession.Settings.CurrentTimePeriod == Settings.TimePeriod.Medieval)
        {
            bolt.SetActive(false);
            fire.SetActive(true);
        }
        else
        {
            bolt.SetActive(true);
            fire.SetActive(false);
        }
    }

    public void Start()
    {
		UIManager.Instance.StateChanged += ToggleUI;
        lookCamera = GameObject.Find("MixedRealityCamera").GetComponent<Camera>();
        SwitchMode(GesturesManager.Instance.IsNavigating);
    }

    private void OnDestroy()
    {
        UIManager.Instance.StateChanged -= ToggleUI;
    }
    public void Update()
    {
        //Look at the user each frame
        transform.rotation = Quaternion.LookRotation(lookCamera.transform.forward);

        //Update the resources ui for city resources.
        if(_lastUpdateTime + UpdateDelay < Time.time)
        {
            CheckForLists();

            if (electricityResources != null)
            {
                //Electricity
                float elecVal = 0;
                float elecInput = 0;
                float elecOut = 0;
                foreach (ResourceData r in electricityResources)
                {
                    elecVal += r.resource.Value;
                    elecInput += r.resource.AddedLastTick;
                    elecOut += r.resource.RecievedLastTick;
                }
                ResourceDataObjects[0].ValueText.text = string.Format("{0:0.00}", elecVal);
                ResourceDataObjects[0].InputText.text = string.Format("{0:0.00}", elecInput);
                ResourceDataObjects[0].OutputText.text = string.Format("{0:0.00}", elecOut); 
            }

            if (waterResources != null)
            {
                //Water
                float waterVal = 0;
                float waterInput = 0;
                float waterOut = 0;
                foreach (ResourceData r in waterResources)
                {
                    waterVal += r.resource.Value;
                    waterInput += r.resource.AddedLastTick;
                    waterOut += r.resource.RecievedLastTick;
                }
                ResourceDataObjects[1].ValueText.text = string.Format("{0:0.00}", waterVal);
                ResourceDataObjects[1].InputText.text = string.Format("{0:0.00}", waterInput);
                ResourceDataObjects[1].OutputText.text = string.Format("{0:0.00}", waterOut);
            }

            _lastUpdateTime = Time.time;
            //Update Money
            FundsText.text = Game.CurrentSession.Settings.Funds.ToString();
            ResidentDemandText.text = Game.CurrentSession.Settings.ResidentialDemand + "";
            ResidentTotal.text = Game.CurrentSession.City.Residents.Count.ToString();
        }
        //Update the happiness value in the UI
        float happiness = WorldGrid.GridSystem.AverageResidentHappiness;
        GridHappinessText.text = string.Format("{0:0.##}", happiness);
        
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
	
	void ToggleUI(int state)
    {
        gameObject.SetActive((state == 0));
    }

    private void CheckForLists()
    {
        if (waterResources == null)
        {
            waterResources = WorldGrid.GridSystem.GetResourceList(typeof(Water));
        }
        if (electricityResources == null)
        {
            electricityResources = WorldGrid.GridSystem.GetResourceList(typeof(Electricity));
        }
    }

    private string GenerateDistrictName()
    {
        string name = "District";
        int i = UnityEngine.Random.Range(0, 10);

        switch (i)
        {
            case 0: name = "Metropolitan District";
                break;
            case 1:
                name = "Upper North Whilgosk";
                break;
            case 2:
                name = "Seckum Hills";
                break;
            case 3:
                name = "North Stant";
                break;
            case 4:
                name = "Liggon Hill";
                break;
            case 5:
                name = "Lower West Meogurd";
                break;
            case 6:
                name = "Straub Heights";
                break;
            case 7:
                name = "Eager Avenue";
                break;
            case 8:
                name = "Matthew's Way";
                break;
            case 9:
                name = "Jon's Square";
                break;
            case 10:
                name = "Clariff Heights";
                break;
        }

        return name;
    }
}


[System.Serializable]
public class ResourceDataInfo
{
    public Text ValueText;
    public Text InputText;
    public Text OutputText;
}

