using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnButtonCall : MonoBehaviour
{
    
    //called from unity button
    public void RespawnPlayers()
    {
        
        DataManager.GetValue<PlayerRespawn>(DataKeys.PLAYER_RESPAWN).Respawn();
    }
}
