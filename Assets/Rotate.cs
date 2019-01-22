using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public float hor = 20.0f;
    public float vert = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * hor);
        transform.Rotate(Vector3.forward, Time.deltaTime * vert);
    }
}
