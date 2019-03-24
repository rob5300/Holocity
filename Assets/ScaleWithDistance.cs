using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithDistance : MonoBehaviour {
    
    private Vector3 scale;
    public Transform player;


    private void Start()
    {
        if (!player) player = Camera.main.transform;

        scale = transform.lossyScale;
    }
        
    public void Update()
    {
        ApplyScale();
    }

    void ApplyScale()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        Vector3 targetScale = scale * Mathf.Clamp((distance / 1.5f), 1f, float.MaxValue);
        targetScale.z = scale.z;
        transform.localScale = targetScale;
    }

}
