using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnchor : MonoBehaviour {

    public Transform AnchorObject;
    public float AngleThreshold;

    private bool _rotating = false;
    private float _time;

	void Start () {
        if (AnchorObject == null) AnchorObject = Camera.main.transform;
        transform.rotation = Quaternion.Euler(0, AnchorObject.rotation.eulerAngles.y, 0);
    }

	void Update () {

        

        if (_rotating) Rotate();
        else Check();

        //Update position
        transform.position = AnchorObject.position;

    }

    private void Check()
    {
        if (Quaternion.Angle(Quaternion.Euler(0, AnchorObject.rotation.eulerAngles.y, 0), transform.rotation) > AngleThreshold)
        {
            _rotating = true;
            _time = 0;
        }
    }

    private void Rotate()
    {
        _time += Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, AnchorObject.rotation.eulerAngles.y, 0), _time);
        if (_time > 0.95) _rotating = false;
    }
}
