using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.WSA;

public class SpacialMapper : MonoBehaviour {
    
    [Header("Update settings")]
    public float UpdateDelay = 2.5f;

    SurfaceObserver surfaceObserver;
    Dictionary<SurfaceId, GameObject> spatialMeshObjects = new Dictionary<SurfaceId, GameObject>();

    public void Start()
    {
        if (XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale))
        {
            // RoomScale mode was set successfully.  App can now assume that y=0 in Unity world coordinate represents the floor.
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError("Was not able to set TrackingSpaceType to RoomScale");
#endif
        }

        surfaceObserver = new SurfaceObserver();

        surfaceObserver.SetVolumeAsAxisAlignedBox(Vector3.zero, new Vector3(3, 3, 3));
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

    private void OnSurfaceChanged(SurfaceId surfaceId, SurfaceChange changeType, Bounds bounds, DateTime updateTime)
    {
        switch (changeType)
        {
            case SurfaceChange.Added:
            case SurfaceChange.Updated:
                if (!spatialMeshObjects.ContainsKey(surfaceId))
                {
                    spatialMeshObjects[surfaceId] = new GameObject("Surface: " + surfaceId.handle);
                    spatialMeshObjects[surfaceId].transform.parent = transform;
                    spatialMeshObjects[surfaceId].AddComponent<MeshRenderer>();
                    spatialMeshObjects[surfaceId].AddComponent<MeshFilter>();
                    spatialMeshObjects[surfaceId].AddComponent<WorldAnchor>();
                    spatialMeshObjects[surfaceId].AddComponent<MeshCollider>();
                }
                GameObject target = spatialMeshObjects[surfaceId];
                SurfaceData sd = new SurfaceData(
                    //the surface id returned from the system
                    surfaceId,
                    //the mesh filter that is populated with the spatial mapping data for this mesh
                    target.GetComponent<MeshFilter>(),
                    //the world anchor used to position the spatial mapping mesh in the world
                    target.GetComponent<WorldAnchor>(),
                    //the mesh collider that is populated with collider data for this mesh, if true is passed to bakeMeshes below
                    target.GetComponent<MeshCollider>(),
                    //triangles per cubic meter requested for this mesh
                    300,
                    //bakeMeshes - if true, the mesh collider is populated, if false, the mesh collider is empty.
                    true
                    );

                surfaceObserver.RequestMeshAsync(sd, OnDataReady);
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
        spatialMeshObjects[bakedData.id].GetComponent<MeshFilter>().mesh = bakedData.outputMesh.mesh;
    }
}
