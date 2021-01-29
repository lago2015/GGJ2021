﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public PlayerController playerA, playerB;
    public Transform spawnA, spawnB;

    public GameObject Canvas, middle,NextLevel,respawn;
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
        NextLevel.SetActive(true);
        respawn.SetActive(false);
        TriggerPlayers(false);
        _callback = callback;
    }
    //Called from unity button
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
            NextLevel.SetActive(false);
            respawn.SetActive(true);
            playerA.transform.position = spawnA.position;
            playerB.transform.position = spawnB.position;
        }
    }
}
