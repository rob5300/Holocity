using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITarget
{

    public Transform targetTransform;
    public RectTransform guideTransform;

    public UITarget(Transform targetTransform, RectTransform guideTransform)
    {
        this.targetTransform = targetTransform;
        this.guideTransform = guideTransform;
    }
}