using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Quaternion objectViewRot;
    public Quaternion roadViewRot;
    public Quaternion mapViewRot;

    private Quaternion carRot;
    private Quaternion camStartingRot;
    private Quaternion offset;
    [SerializeField] private CameraView currentView;

    private enum CameraView
    {
        left,
        middle,
        right
    }

    private void Start()
    {
        currentView = CameraView.middle;
        camStartingRot = Camera.main.transform.rotation;
    }

    private void Update()
    {
        offset = new Quaternion(camStartingRot.x - Camera.main.transform.rotation.x,
            camStartingRot.y - Camera.main.transform.rotation.y,
            camStartingRot.z - Camera.main.transform.rotation.z,
            camStartingRot.w - Camera.main.transform.rotation.w);

        TurnCamera();
    }

    

    private void TurnCamera()
    {
        if (Input.mousePosition.x < 10 && currentView != CameraView.left)
        {
            OrientMapView();
        }
        else if (Input.mousePosition.x > Screen.width - 10 && currentView != CameraView.right)
        {
            OrientObjView();
        }
        else if (Input.mousePosition.x >= 10 && Input.mousePosition.x <= Screen.width - 10 && currentView != CameraView.middle)
        {
            OrientRoadView();
        }
    }

    [ContextMenu("orient to object View")]
    public void OrientObjView()
    {
        currentView = CameraView.right;
        Camera.main.transform.localRotation = objectViewRot;
    }

    [ContextMenu("orient to road View")]
    public void OrientRoadView()
    {
        currentView = CameraView.middle;
        Camera.main.transform.localRotation = roadViewRot;
    }

    [ContextMenu("orient to map View")]
    public void OrientMapView()
    {
        currentView = CameraView.left;
        Camera.main.transform.localRotation = mapViewRot;
    }

    [ContextMenu("quaternion check")]
    public void QuaternionCheck()
    {
        Debug.Log("normal cam rot: " + Camera.main.transform.rotation);
        Debug.Log("starting cam rot: " + camStartingRot);
        Debug.Log("LOCAL cam rot: " + Camera.main.transform.localRotation);
        Debug.Log("obj view: " + objectViewRot);
        Debug.Log("road view: " + roadViewRot);
        Debug.Log("map view: " + mapViewRot);
        Debug.Log("offset: " + offset);
    }
}
