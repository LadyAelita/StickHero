using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundHandler : MonoBehaviour
{
    // UnityEditor reference fields
    public Camera mainCamera;
    public GameObject backgroundRight;
    public SpriteRenderer backgroundSprite;

    // Private fields
    private GameObject backgroundLeft;
    private float spriteWidth;

    private bool isPositionOutsideLeftCameraBound(Vector3 pos, Camera camera)
    {
        Vector3 viewportPos = camera.WorldToViewportPoint(pos);

        return viewportPos.x < 0.0f;
    }

    private float getSpriteWidth(SpriteRenderer sprite)
    {
        return sprite.bounds.size.x;
    }

    private void shiftToNextBackground()
    {
        transform.position += new Vector3(spriteWidth, 0.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteWidth = getSpriteWidth(backgroundSprite);

        backgroundLeft = Instantiate(backgroundRight, transform);
        backgroundLeft.transform.position -= new Vector3(spriteWidth, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPositionOutsideLeftCameraBound(transform.position, mainCamera))
        {
            shiftToNextBackground();
        }
    }
}
