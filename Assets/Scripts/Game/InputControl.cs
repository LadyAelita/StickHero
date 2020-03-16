using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    // UnityEditor fields
    public TerrainGenerator terrainGen;
    public PlayerCharacter player;
    public CameraControl camControl;
    public ScoreCounter scoreCounter;

    // Private fields
    private bool lastTouchStatus = false;

    private enum Stage
    {
        Waiting = 1,
        StickGrowing = 2,
        StickFalling = 3,
        Walking = 4,
        Dying = 5,
        Shifting = 6
    }
    private Stage stage = Stage.Waiting;

    private bool IsScreenTouched()
    {
        // Regular touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) return true;

        // Mouse button
        if (Input.GetMouseButton(0)) return true;

        return false;
    }

    private void HandlePress()
    {
        if (stage == Stage.Waiting)
        {
            player.Crouch();
            terrainGen.SpawnStick();
            stage = Stage.StickGrowing;
        }
    }

    private void HandleRelease()
    {
        if (stage == Stage.StickGrowing)
        {
            terrainGen.StickFall();
            stage = Stage.StickFalling;
        }
    }

    private void HandleStickFall()
    {
        stage = Stage.Walking;
        player.Walk();
    }

    private void HandleShiftingEnd(Transform camTransform)
    {
        stage = Stage.Waiting;
    }

    private void HandleScoreIncrement()
    {
        scoreCounter.score += 1;
    }

    private void HandleStopWalking()
    {
        if (player.IsDead())
        {
            stage = Stage.Dying;
            HandleGameOver();
        }
        else
        {
            stage = Stage.Shifting;
            HandleScoreIncrement();
            float newCameraX = camControl.AnimateShiftToPlayer(HandleShiftingEnd);
            float newCameraRightEdge = newCameraX + camControl.cameraHalfWidth;
            terrainGen.SpawnAndAnimateNextPlatform(newCameraRightEdge);
        }
    }

    private void HandleGameOver()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool screenTouched = IsScreenTouched();
        if (screenTouched != lastTouchStatus)
        {
            if (screenTouched)
            {
                HandlePress();
            }
            else
            {
                HandleRelease();
            }
        }
        lastTouchStatus = screenTouched;

        if (stage == Stage.StickFalling)
        {
            if (terrainGen.IsStickInAir() == false)
            {
                HandleStickFall();
            }
        }

        if (stage == Stage.Walking)
        {
            if (player.IsWalking() == false)
            {
                HandleStopWalking();
            }
        }
    }
}
