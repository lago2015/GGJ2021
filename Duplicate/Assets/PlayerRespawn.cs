using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public PlayerController playerA, playerB;
    public Transform spawnA, spawnB;

    public GameObject Canvas, middle;
    public TMPro.TextMeshProUGUI text;

    private EndOfLevelArena _callback = null;

    private void OnTriggerEnter2D(Collider2D other) {
        // Trigger death.
        TriggerPlayers(true);

        text.text = "DED";
        Canvas.SetActive(true);
    }

    public void PlayerWon(EndOfLevelArena callback) {
        text.text = "WUN";
        Canvas.SetActive(true);
        TriggerPlayers(false);
        _callback = callback;
    }

    public void Respawn() {
        playerA.gameObject.SetActive(true);
        playerB.gameObject.SetActive(true);
        middle.SetActive(true);

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
}
