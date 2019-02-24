using UnityEngine;
using HoloToolkit.Unity;

public class GazeFollow : MonoBehaviour {

    public Transform AnchorObject;
    public Transform childTransform;
    public float AngleThreshold;
    public Billboard billboard;

    private Vector3 offset = new Vector3(0, 0, 1.5f);
    private bool _rotating = false;
    private float _time;

    void Start()
    {
        if (AnchorObject == null) AnchorObject = Camera.main.transform;
        transform.rotation = Quaternion.Euler(0, AnchorObject.rotation.eulerAngles.y, 0);
        
        childTransform.localPosition = offset;
    }

    void Update()
    {
        Debug.Log(UIManager.Instance.menuState);
        if (UIManager.Instance.menuState != UIManager.MenuState.MainMenu)
        {
            childTransform.localPosition = Vector3.zero;
          
            return;
        }
        else if(childTransform.localPosition != offset)
        {
            childTransform.localPosition = offset;
        }


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
