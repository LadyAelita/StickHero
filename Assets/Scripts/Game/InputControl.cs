using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputControl : MonoBehaviour
{
    // UnityEditor fields
    public TerrainGenerator terrainGen;
    public PlayerCharacter player;
    public CameraControl camControl;
    public ScoreCounter scoreCounter;
    public GameOverHandler gameOverHandler;

    // Private fields
    private bool lastTouchStatus = false;

    private enum Stage
    {
        Waiting = 1,
        StickGrowing = 2,
        StickFalling = 3,
        Walking = 4,
        Dying = 5,
        Shifting = 6,
        GameOver = 7
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

        if (terrainGen.IsStickEndOnLastCenterMark())
        {
            scoreCounter.score += 1;
        }
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
        gameOverHandler.UpdateScores(scoreCounter.score);
        gameOverHandler.DisplayOverlay();
        stage = Stage.GameOver;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
        else if (stage == Stage.Walking)
        {
            if (player.IsWalking() == false)
            {
                HandleStopWalking();
            }
        }
        else if (stage == Stage.Dying)
        {
            if (player.transform.position.y < -camControl.GetCameraHalfHeight())
            {
                HandleGameOver();
            }
        }
    }
}
