using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class AddTarget : MonoBehaviour
{

    public EdgeGuidance edgeGuidance;
    public Sprite icon;
    public Color color;

    // Use this for initialization
    void Start()
    {

        edgeGuidance.AddTarget(transform, icon, color);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
