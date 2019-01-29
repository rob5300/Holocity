using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITarget
{

    public Transform targetTransform;
    public Transform guideTransform;

    public UITarget(Transform targetTransform, Transform guideTransform)
    {
        this.targetTransform = targetTransform;
        this.guideTransform = guideTransform;
    }
}