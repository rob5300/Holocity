using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Settings;

public class AssetCache
{
    public GameObject TileBorder;
    public GameObject MoveButton;
    public GameObject ScaleButton;
    public GameObject RotateButton;
    public GameObject NewGridButton;
    public GameObject ResourceUI;
    public GameObject ElectricityWarning;
    public GameObject WaterWarning;


    public AssetCache(GameSettings gameSettings)
    {
        TileBorder = Resources.Load<GameObject>("Tile Border");
        MoveButton = Resources.Load<GameObject>("Move Button");
        RotateButton = Resources.Load<GameObject>("Rotate Button");
        NewGridButton = Resources.Load<GameObject>("New Grid Button");
        ScaleButton = Resources.Load<GameObject>("Scale Button");
        ResourceUI = Resources.Load<GameObject>("UI/Resource UI");

        if (gameSettings.CurrentTimePeriod == TimePeriod.Medieval)
        {
            ElectricityWarning = Resources.Load<GameObject>("FireWarning");
        }
        else
        {
            ElectricityWarning = Resources.Load<GameObject>("ElectricityWarning");
        }
        WaterWarning = Resources.Load<GameObject>("WaterWarning");
    }
}
