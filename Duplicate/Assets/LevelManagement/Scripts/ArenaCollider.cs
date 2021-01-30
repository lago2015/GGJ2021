using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaCollider : MonoBehaviour
{
    public EndOfLevelArena callback;
    public BoxCollider2D box;
    [Header("Left Side Only")]
    public bool leftSide;

    private void Awake()
    {
        if(leftSide)
        {
            DataManager.SetValue(DataKeys.PLAYERA_FINISHLINE,transform.position);    
        }
        else
        {
            DataManager.SetValue(DataKeys.PLAYERB_FINISHLINE,transform.position);
        }
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        callback.PlayerTriggered(leftSide, box);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, box.size);
    }
}
