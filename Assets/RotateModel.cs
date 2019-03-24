using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateModel : MonoBehaviour {


    private void Update()
    {
        transform.Rotate(Vector3.up, 20.0f * Time.deltaTime);
    }

}
