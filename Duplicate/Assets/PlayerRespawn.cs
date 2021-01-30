using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerRespawn : MonoBehaviour
{
    public PlayerController playerA, playerB;
    public Animator uiAnimator;

    public Transform spawnA, spawnB;
    [InlineEditor] public FusionSequence FusionSequence;

    //Assigned by DeathZoneAssigner
    [HideInInspector] public GameObject EndGameCanvas, middle, NextLevel, respawn, restart;
    [HideInInspector] public TMPro.TextMeshProUGUI EndGameMessage;

    private EndOfLevelArena _callback = null;
    private CountdownStartGame _countdownRef;

    private void Awake()
    {
        _countdownRef = FindObjectOfType<CountdownStartGame>();
        DataManager.SetValue(DataKeys.PLAYER_RESPAWN, this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Trigger death.
        GameOver();
    }

    public void GameOver()
    {
        TriggerPlayers(true);
        SetUpGameEndScreen(false);
    }

    public void StartGame()
    {
        playerA.IsActive = true;
        playerB.IsActive = true;
    }

    public void PlayerWon(EndOfLevelArena callback)
    {
        TriggerPlayers(false);

        // Play Fusion Sequence and then
        FusionSequence.MainSequence()
                        .Play()
                        .OnComplete(() => Debug.Log("Complete!"));

        // SetUpGameEndScreen(true); // TODO maybe call this, or something else to trigger a leveltransition.

        _callback = callback;
    }

    public void Respawn()
    {
        playerA.transform.position = spawnA.position;
        playerB.transform.position = spawnB.position;
        playerA.gameObject.SetActive(true);
        playerB.gameObject.SetActive(true);
        middle.SetActive(true);

        EndGameCanvas.SetActive(false);
        if (_callback != null)
        {
            _callback.cameraChange.swapback = true;
            _callback = null;
        }

        if (_countdownRef)
        {
            _countdownRef.SetUpCountdown();
        }
    }

    private void TriggerPlayers(bool dead)
    {
        playerB.IsActive = playerA.IsActive = false;
        middle.SetActive(false);
        if (dead)
        {
            playerA.gameObject.SetActive(false);
            playerB.gameObject.SetActive(false);
            playerA.transform.position = spawnA.position;
            playerB.transform.position = spawnB.position;
        }
    }

    private void SetUpGameEndScreen(bool gameWon)
    {
        if (gameWon)
        {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            if (currentLevel + 1 == SceneManager.sceneCountInBuildSettings)
            {
                //Game Complete
                EndGameMessage.text = "Thank you for playing !";
                restart.SetActive(true);
                NextLevel.SetActive(false);
                respawn.SetActive(false);
                uiAnimator.SetTrigger("GameBeaten");
            }
            else
            {
                //Next Level
                EndGameMessage.text = "WUN";
                NextLevel.SetActive(true);
                restart.SetActive(false);
                respawn.SetActive(false);
            }
        }
        else
        {
            //Player lost
            EndGameMessage.text = "DED";
            respawn.SetActive(true);
            restart.SetActive(false);
            NextLevel.SetActive(false);
            uiAnimator.SetTrigger("GameLost");
        }
        // EndGameCanvas.SetActive(true);
    }
}
