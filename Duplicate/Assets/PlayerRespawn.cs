using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerRespawn : MonoBehaviour
{
    [HideInInspector] public Animator uiAnimator;

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
        SetUpGameOverScreen();
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
                        .Play()
                        .OnComplete(()=>DataManager.MakeItRain<SceneTransition>(DataKeys.SCENE_TRANSITION).NextLevelWithDelay());
        
        _callback = callback;
    }
    
    public void Respawn() {
        uiAnimator.SetTrigger("Reset");
        
        _playerA.transform.position = spawnA.position;
        _playerB.transform.position = spawnB.position;
        _playerA.gameObject.SetActive(true);
        _playerB.gameObject.SetActive(true);
        middle.SetActive(true);

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

    public void SetUpGameCompleteScreen()
    {
        //Game Complete
        EndGameMessage.text = "Thank you for playing !";
        restart.SetActive(true);
        NextLevel.SetActive(false);
        respawn.SetActive(false);       
        uiAnimator.SetTrigger("GameBeaten");
    }

    public void SetUpGameOverScreen()
    {
        //Player lost
        EndGameMessage.text = "DED";
        respawn.SetActive(true);
        restart.SetActive(false);
        NextLevel.SetActive(false);
        uiAnimator.SetTrigger("GameLost");   
    }
}
