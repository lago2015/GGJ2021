using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerRespawn : MonoBehaviour
{
    public Animator uiAnimator;

    public Transform spawnA, spawnB;

    //Assigned by DeathZoneAssigner
    [HideInInspector] public GameObject EndGameCanvas, middle, NextLevel, respawn, restart;
    [HideInInspector] public TMPro.TextMeshProUGUI EndGameMessage;

    private EndOfLevelArena _callback = null;
    
    private PlayerController _playerA;
    private PlayerController _playerB;
    private void Awake()
    {
        DataManager.ToTheCloud(DataKeys.PLAYER_RESPAWN, this);
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
        if(!_playerA || !_playerB)
        {
            _playerA = DataManager.MakeItRain<PlayerController>(DataKeys.PLAYERA);
            _playerB = DataManager.MakeItRain<PlayerController>(DataKeys.PLAYERB);
        }
        _playerA.IsActive = true;
        _playerB.IsActive = true;
        DataManager.MakeItRain<ProgressBar>(DataKeys.PROGRESSBAR).IsActive = true;
    }

    public void PlayerWon(EndOfLevelArena callback)
    {
        TriggerPlayers(false);

        // Play Fusion Sequence and then
        DataManager.MakeItRain<FusionSequence>(DataKeys.FUSION_SEQUENCE)
                        .MainSequence()
                        .Play();
        DataManager.MakeItRain<SceneTransition>(DataKeys.SCENE_TRANSITION).NextLevel();
        _callback = callback;
    }
    
    public void Respawn() {
        uiAnimator.SetTrigger("Reset");
        
        _playerA.transform.position = spawnA.position;
        _playerB.transform.position = spawnB.position;
        _playerA.gameObject.SetActive(true);
        _playerB.gameObject.SetActive(true);
        middle.SetActive(true);

        EndGameCanvas.SetActive(false);
        if (_callback != null)
        {
            _callback.cameraChange.swapback = true;
            _callback = null;
        }

        DataManager.MakeItRain<CountdownStartGame>(DataKeys.COUNTDOWN_STARTGAME).SetUpCountdown();
    }

    private void TriggerPlayers(bool dead) {
        _playerB.IsActive = _playerA.IsActive = false;
        middle.SetActive(false);
        if(dead) {
            _playerA.gameObject.SetActive(false);
            _playerB.gameObject.SetActive(false);
            _playerA.transform.position = spawnA.position;
            _playerB.transform.position = spawnB.position;
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
                uiAnimator.SetTrigger("LevelWon");
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
    }
}
