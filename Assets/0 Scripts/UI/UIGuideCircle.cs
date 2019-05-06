using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuideCircle : MonoBehaviour {

    public static UIGuideCircle instance;

    public GameObject GuidePrefab;

    List<UITarget> Targets = new List<UITarget>();
    
    float ScaleMultiplier = 100f;
    Camera mainCamera;
    Vector2 CamBound;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mainCamera = transform.GetComponent<Camera>();
        CamBound = new Vector2(mainCamera.pixelWidth, mainCamera.pixelHeight) / 2;
    }

    void Update()
    {
        UpdateGuide();
    }

    void UpdateGuide()
    {
        foreach (UITarget target in Targets)
        {
            bool OutOfView = CheckOutOfView(target.targetTransform);

            if (OutOfView && !target.guideTransform.gameObject.activeSelf)
            {
                target.guideTransform.gameObject.SetActive(true);
            }
            else if(!OutOfView)
            {
                target.guideTransform.gameObject.SetActive(false);
                continue;
            }

            UpdateGuideAlpha(target);
            UpdateGuideTransform(target);
        }
    }

    void UpdateGuideAlpha(UITarget target)
    {
        Vector3 targetDir = target.targetTransform.position - mainCamera.transform.position;
        float angle = Mathf.Abs(Vector3.SignedAngle(targetDir, mainCamera.transform.forward, Vector3.up));

        Color colour = target.guideTransform.GetComponent<Renderer>().material.color;
        colour.a = Mathf.Clamp((1f / 180f) * angle, 0.02f, 1.0f);

        target.guideTransform.GetComponent<Renderer>().material.color = colour;
    }

    void UpdateGuideTransform(UITarget target)
    {
        //scale
        float scale = Vector3.Distance(target.targetTransform.position, mainCamera.transform.position);
        target.guideTransform.localScale = new Vector3(ScaleMultiplier * scale, ScaleMultiplier / 5f,ScaleMultiplier * scale);

        //position
        Vector3 pos = mainCamera.transform.position;
        pos.y = target.targetTransform.position.y;
        target.guideTransform.position = pos;

        //Rotation
        target.guideTransform.rotation = Quaternion.identity;

    }

    //Update as it uses centre of grid atm
    bool CheckOutOfView(Transform target)
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(target.position);
        bool OutOfView = !(screenPoint.z > 0 && screenPoint.x > 0
            && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1);
        
        return OutOfView;
    }
    
    public void AddTarget(Transform target)
    {
        //Instantiate the circle
        GameObject targetGuide = Instantiate(GuidePrefab, transform);
        targetGuide.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);

        Targets.Add(new UITarget(target, targetGuide.transform));

        targetGuide.SetActive(true);
    }

   
    public void RemoveTarget(Transform targetTransform)
    {
        foreach (UITarget target in Targets)
        {
            if (target.targetTransform == targetTransform)
            {
                Targets.Remove(target);
                Destroy(target.guideTransform.gameObject);
                return;
            }
        }
    }

}


