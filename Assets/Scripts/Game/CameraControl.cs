using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public AnimationControl animControl;
    public GameObject playerCharacterObject;

    private Camera cam;
    private Vector3 cameraStartingLocation;

    private float deltaXFromPlayer;

    public float cameraHalfWidth { get; private set; }
    public float cameraWidth { get; private set; }

    public float GetCameraHalfHeight()
    {
        return cam.orthographicSize;
    }

    public float GetCameraRightEdge()
    {
        return cam.ViewportToWorldPoint(new Vector3(1.0f, 0.0f)).x;
    }

    public void MoveCameraToStart()
    {
        transform.position = cameraStartingLocation;
    }

    public float AnimateShiftToPlayer(AnimationControl.AnimationCallback callback=null)
    {
        float targetX = playerCharacterObject.transform.position.x + deltaXFromPlayer;
        Vector3 targetPos = new Vector3(targetX, transform.position.y, transform.position.z);

        animControl.AnimateTransformPos(transform, transform.position, targetPos, callback);
        return targetX;
    }

    void Awake()
    {
        cam = GetComponent<Camera>();
        cameraHalfWidth = cam.orthographicSize * cam.aspect;
        cameraWidth = 2 * cameraHalfWidth;

        // Store the starting location
        cameraStartingLocation = transform.position + new Vector3(cameraHalfWidth, 0.0f);
        MoveCameraToStart();
    }

    // Start is called before the first frame update
    void Start()
    {
        deltaXFromPlayer = transform.position.x - playerCharacterObject.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
