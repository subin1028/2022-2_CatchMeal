using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARFaceSwitch : MonoBehaviour
{
    private ARFaceManager aRFaceManager;
    private Material currentMaterial;

    private void Awake()
    {
        aRFaceManager = GetComponent<ARFaceManager>();
        currentMaterial = aRFaceManager.facePrefab.GetComponent<MeshRenderer>().material;
    }

    public void UpdateFaceMaterial(Material material)
    {
        currentMaterial = material;
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (ARFace face in aRFaceManager.trackables)
        {
            face.GetComponent<MeshRenderer>().material = currentMaterial;
        }
    }
}
