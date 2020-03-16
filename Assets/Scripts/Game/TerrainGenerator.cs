using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // Unity Editor fields
    public Camera mainCamera;
    public GameObject startingPlatform;
    public GameObject centerMarkBlueprint;
    public AnimationControl animControl;
    public float minimalPlatformWidth;
    public float maximalPlatformWidth;
    public float minimalPlatformToPlatformSpacing;
    public float minimalPlatformToRightEdgeSpacing;

    // Private fields
    private GameObject lastPlatform;
    private CameraControl camControl;

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

    private GameObject InstantiateNewPlatform(float spawnPosLocalX, float scaleX)
    {
        GameObject platform = Instantiate(startingPlatform, transform);
        platform.transform.localPosition = new Vector3(spawnPosLocalX, 0.0f);

        Vector3 platformLocalScale = platform.transform.localScale;
        platform.transform.localScale = new Vector3(scaleX, platformLocalScale.y, platformLocalScale.z);

        return platform;
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

        SpawnAndAnimateNextPlatform();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
