using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnButtonCall : MonoBehaviour
{
    private PlayerRespawn _playerRespawn;
    private void Awake()
    {
        _playerRespawn = FindObjectOfType<PlayerRespawn>();
    }
    //called from unity button
    public void RespawnPlayers()
    {
        if(!_playerRespawn) return;
        _playerRespawn.Respawn();
    }
}
