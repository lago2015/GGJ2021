using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerRespawn : MonoBehaviour
{
    public PlayerController playerA, playerB;
    public Transform spawnA, spawnB;

    public GameObject Canvas, middle,NextLevel,respawn,restart;
    public TMPro.TextMeshProUGUI EndGameMessage;

    private EndOfLevelArena _callback = null;

    private void OnTriggerEnter2D(Collider2D other) {
        // Trigger death.
        TriggerPlayers(true);
        SetUpGameEndScreen(false);
        
    }

    public void PlayerWon(EndOfLevelArena callback)
    {
        TriggerPlayers(false);
        SetUpGameEndScreen(true);
        _callback = callback;
    }
    
    public void Respawn() {
        playerA.transform.position = spawnA.position;
        playerB.transform.position = spawnB.position;
        playerA.gameObject.SetActive(true);
        playerB.gameObject.SetActive(true);
        middle.SetActive(true);

        Canvas.SetActive(false);
        if(_callback != null) { 
            _callback.cameraChange.swapback = true;
            _callback = null;
        }
    }

    private void TriggerPlayers(bool dead) {
        playerB.IsActive = playerA.IsActive = false;
        middle.SetActive(false);
        if(dead) {
            playerA.gameObject.SetActive(false);
            playerB.gameObject.SetActive(false);
            playerA.transform.position = spawnA.position;
            playerB.transform.position = spawnB.position;
        }
    }

    private void SetUpGameEndScreen(bool gameWon)
    {
        if(gameWon)
        {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            if(currentLevel + 1 > SceneManager.sceneCount)
            {
                //Game Complete
                EndGameMessage.text = "Thank you for playing !";
                restart.SetActive(true);
                NextLevel.SetActive(false);
                respawn.SetActive(false);
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
        }
        Canvas.SetActive(true);
    }
}
