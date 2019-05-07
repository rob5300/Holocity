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
    public Toggle move;
    public Toggle rotate;
    public Color ToggleColor;

    private float _lastUpdateTime = -999;
    [NonSerialized]
    public WorldGrid WorldGrid;

    private Camera lookCamera;
    private List<ResourceData> waterResources;
    private List<ResourceData> electricityResources;

    public void Start()
    {
		UIManager.Instance.StateChanged += ToggleUI;
        lookCamera = GameObject.Find("MixedRealityCamera").GetComponent<Camera>();
        SwitchMode(GesturesManager.Instance.IsNavigating);
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
}


[System.Serializable]
public class ResourceDataInfo
{
    public Text ValueText;
    public Text InputText;
    public Text OutputText;
}

