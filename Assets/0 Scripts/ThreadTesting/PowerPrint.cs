using Infrastructure.Grid.Entities.Buildings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPrint : MonoBehaviour {

    public PowerPlant plant;

    private float LastPrintTime = -999;

	void Update () {
        if (LastPrintTime + 1 < Time.time)
        {
            Debug.Log(plant.Power);
            LastPrintTime = Time.time;
        }
	}
}
