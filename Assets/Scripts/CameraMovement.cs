using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Quaternion objectViewRot;
    public Quaternion roadViewRot;
    public Quaternion mapViewRot;

    [ContextMenu("orient to object View")]
    public void OrientObjView()
    {
        Camera.main.transform.rotation = objectViewRot;
    }

    [ContextMenu("orient to road View")]
    public void OrientRoadView()
    {
        Camera.main.transform.rotation = roadViewRot;
    }

    [ContextMenu("orient to map View")]
    public void OrientMapView()
    {
        Camera.main.transform.rotation = mapViewRot;
    }

}
