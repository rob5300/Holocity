using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.WSA;

public class SpacialMapper : MonoBehaviour {
    
    [Header("Volume area points")]
    public Transform OriginPoint;
    public Transform ExtentPoint;
#if UNITY_EDITOR
    public bool DrawAreaGizmo = true;
#endif
    [Header("Update settings")]
    public float UpdateDelay = 2.5f;

    SurfaceObserver surfaceObserver;
    Dictionary<SurfaceId, GameObject> spatialMeshObjects = new Dictionary<SurfaceId, GameObject>();

    public void Awake()
    {
        surfaceObserver = new SurfaceObserver();
        if(OriginPoint == null || ExtentPoint == null)
        {
            Debug.LogError("Volume area transform(s) missing!");
        }
    }

    public void Start()
    {
        surfaceObserver.SetVolumeAsAxisAlignedBox(OriginPoint.transform.position, ExtentPoint.transform.position);
        StartCoroutine(UpdateLoop());
    }

    IEnumerator UpdateLoop()
    {
        var wait = new WaitForSeconds(UpdateDelay);
        while (true)
        {
            surfaceObserver.Update(OnSurfaceChanged);
            yield return wait;
        }
    }

    private void OnSurfaceChanged(SurfaceId surfaceId, SurfaceChange changeType, Bounds bounds, System.DateTime updateTime)
    {
        switch (changeType)
        {
            case SurfaceChange.Added:
            case SurfaceChange.Updated:
                if (!spatialMeshObjects.ContainsKey(surfaceId))
                {
                    spatialMeshObjects[surfaceId] = new GameObject("spatial-mapping-" + surfaceId);
                    spatialMeshObjects[surfaceId].transform.parent = this.transform;
                    spatialMeshObjects[surfaceId].AddComponent<MeshRenderer>();
                }
                GameObject target = spatialMeshObjects[surfaceId];
                SurfaceData sd = new SurfaceData(
                    //the surface id returned from the system
                    surfaceId,
                    //the mesh filter that is populated with the spatial mapping data for this mesh
                    target.GetComponent<MeshFilter>() ?? target.AddComponent<MeshFilter>(),
                    //the world anchor used to position the spatial mapping mesh in the world
                    target.GetComponent<WorldAnchor>() ?? target.AddComponent<WorldAnchor>(),
                    //the mesh collider that is populated with collider data for this mesh, if true is passed to bakeMeshes below
                    target.GetComponent<MeshCollider>() ?? target.AddComponent<MeshCollider>(),
                    //triangles per cubic meter requested for this mesh
                    1000,
                    //bakeMeshes - if true, the mesh collider is populated, if false, the mesh collider is empty.
                    true
                    );

                //surfaceObserver.RequestMeshAsync(sd, OnDataReady);
                break;
            case SurfaceChange.Removed:
                var obj = spatialMeshObjects[surfaceId];
                spatialMeshObjects.Remove(surfaceId);
                if (obj != null)
                {
                    GameObject.Destroy(obj);
                }
                break;
            default:
                break;
        }
    }

    private void OnDataReady(SurfaceData bakedData, bool outputWritten, float elapsedBakeTimeSeconds)
    {
        
    }


#if UNITY_EDITOR
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Pickable | GizmoType.NotInSelectionHierarchy)]
    public void OnDrawGizmos()
    {
        if (!DrawAreaGizmo) return;
        Vector3 center = Vector3.Lerp(OriginPoint.transform.position, ExtentPoint.transform.position, 0.5f);
        Gizmos.DrawWireCube(center, (OriginPoint.transform.position - ExtentPoint.transform.position));
    }
#endif
}
