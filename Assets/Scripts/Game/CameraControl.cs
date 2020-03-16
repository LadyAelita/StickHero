using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public AnimationControl animControl;

    private Camera cam;
    private float cameraHalfWidth;
    private Vector3 cameraStartingLocation;

    public float GetCameraRightEdge()
    {
        return cam.ViewportToWorldPoint(new Vector3(1.0f, 0.0f)).x;
    }

    public void MoveCameraToStart()
    {
        transform.position = cameraStartingLocation;
    }

    void Awake()
    {
        cam = GetComponent<Camera>();
        cameraHalfWidth = cam.orthographicSize * cam.aspect;

        // Store the starting location
        cameraStartingLocation = transform.position + new Vector3(cameraHalfWidth, 0.0f);

        MoveCameraToStart();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
