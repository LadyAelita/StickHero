using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    // Unity Editor fields
    public SpriteRenderer startingPlatformSprite;
    public TerrainGenerator terrainGen;
    public float walkingTime;

    // Private fields
    private float distanceFromEdge;
    private Animator animator;
    private Rigidbody2D body;

    private bool walking = false;
    private bool dead = false;
    private float targetX;
    private float stickEndX;
    private float lastPlatformStartX;
    private float lastPlatformEdge;
    private float velocityX = 0.0f;

    public void Walk()
    {
        lastPlatformStartX = terrainGen.GetLastPlatformX();
        lastPlatformEdge = terrainGen.GetLastPlatformEdge();

        float perfectTargetX = lastPlatformEdge - distanceFromEdge;
        stickEndX = terrainGen.GetPrevPlatformEdge() + terrainGen.GetLastStickLength();

        targetX = Mathf.Max(perfectTargetX, stickEndX);

        float deltaX = targetX - transform.position.x;
        velocityX = deltaX / walkingTime;

        walking = true;
        animator.SetBool("walk", true);
        body.velocity = new Vector2(velocityX, body.velocity.y);
    }

    public void Crouch()
    {
        animator.SetBool("crouch", true);
    }

    public bool IsWalking()
    {
        return walking;
    }

    public bool IsDead()
    {
        return dead;
    }

    private void HandleWalkingStop()
    {
        velocityX = 0.0f;
        walking = false;
        animator.SetBool("walk", false);
    }

    private void HandleDeath()
    {
        terrainGen.RemoveLastStickCollision();
        terrainGen.RemoveLastPlatformCollision();
        dead = true;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        distanceFromEdge = startingPlatformSprite.bounds.size.x - transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (walking)
        {
            float posX = transform.position.x;
            if (posX > stickEndX && (posX < lastPlatformStartX || posX > lastPlatformEdge))
            {
                HandleWalkingStop();
                HandleDeath();
            }
            else if (posX >= targetX)
            {
                transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
                HandleWalkingStop();
            }
            body.velocity = new Vector2(velocityX, body.velocity.y);
        }
    }
}
