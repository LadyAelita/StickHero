using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // Unity Editor fields
    public Camera mainCamera;
    public GameObject startingPlatform;
    public GameObject centerMarkBlueprint;
    public GameObject stickBlueprint;
    public AnimationControl animControl;
    public float minimalPlatformWidth;
    public float maximalPlatformWidth;
    public float minimalPlatformToPlatformSpacing;
    public float minimalPlatformToRightEdgeSpacing;
    public float stickGrowthSpeed;

    // Private fields
    private GameObject lastPlatform;
    private GameObject lastStick;
    private CameraControl camControl;

    private bool stickGrowing = false;
    private bool stickInAir = false;

    private float GetMinimalLocalX()
    {
        SpriteRenderer lastPlatformSprite = lastPlatform.GetComponent<SpriteRenderer>();
        float lastPlatformWidth = lastPlatformSprite.bounds.size.x;

        return lastPlatform.transform.localPosition.x + lastPlatformWidth + minimalPlatformToPlatformSpacing;
    }

    private float GetMaximalLocalX()
    {
        return camControl.GetCameraRightEdge() - minimalPlatformToRightEdgeSpacing;
    }

    private GameObject InstantiateNewStick()
    {
        SpriteRenderer lastPlatformSprite = lastPlatform.GetComponent<SpriteRenderer>();
        float lastPlatformWidth = lastPlatformSprite.bounds.size.x;
        Vector3 lastPlatformLocalEdge = lastPlatform.transform.localPosition + new Vector3(lastPlatformWidth, 0.0f);

        GameObject stick = Instantiate(stickBlueprint, transform);
        stick.transform.localPosition = lastPlatformLocalEdge;

        return stick;
    }

    private GameObject InstantiateNewPlatform(float spawnPosLocalX, float scaleX)
    {
        GameObject platform = Instantiate(startingPlatform, transform);
        platform.transform.localPosition = new Vector3(spawnPosLocalX, 0.0f);

        Vector3 platformLocalScale = platform.transform.localScale;
        platform.transform.localScale = new Vector3(scaleX, platformLocalScale.y, platformLocalScale.z);

        return platform;
    }

    public void SpawnStick()
    {
        GameObject stick = InstantiateNewStick();
        lastStick = stick;
        stickGrowing = true;
        stickInAir = true;
    }

    private void _StickHasFallenCallback(Transform stickTransform)
    {
        stickGrowing = false;
        stickInAir = false;
        lastStick = null;
    }

    public void StickFall()
    {
        if (lastStick == null) return;

        stickGrowing = false;
        Vector3 newEuler = new Vector3(0.0f, 0.0f, -90.0f);
        animControl.AnimateTransformEuler(lastStick.transform, lastStick.transform.eulerAngles, newEuler, _StickHasFallenCallback);
    }

    public bool IsStickInAir()
    {
        return stickInAir;
    }

    public void SpawnAndAnimateNextPlatform()
    {
        float minX = GetMinimalLocalX();
        float maxX = GetMaximalLocalX();
        float minWidth = minimalPlatformWidth;
        float maxWidth = Mathf.Min(maxX - minX, maximalPlatformWidth);

        float width = Random.Range(minWidth, maxWidth);
        float localX = Random.Range(minX, maxX - width); // Assuming pivot on left edge

        GameObject platform = InstantiateNewPlatform(camControl.GetCameraRightEdge(), width);

        Vector3 targetPos = transform.TransformPoint(new Vector3(localX, 0.0f));
        animControl.AnimateTransformPos(platform.transform, platform.transform.position, targetPos);

        lastPlatform = platform;
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPlatform = startingPlatform;
        camControl = mainCamera.GetComponent<CameraControl>();

        //SpawnAndAnimateNextPlatform();

        SpawnStick();
    }

    // Update is called once per frame
    void Update()
    {
        if (stickGrowing)
        {
            lastStick.transform.localScale += new Vector3(0.0f, stickGrowthSpeed * Time.deltaTime);

            // Stop stick growth roughly after it's length exceeds the viewport's width
            if (lastStick.transform.localScale.y >= camControl.cameraWidth)
            {
                stickGrowing = false;
            }
        }
    }
}
