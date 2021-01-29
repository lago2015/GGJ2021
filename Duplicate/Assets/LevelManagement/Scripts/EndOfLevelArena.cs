using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevelArena : MonoBehaviour
{
    private bool _left, _right;
    public CameraChange cameraChange;
    public List<Collider2D> callbacks;

    public PlayerRespawn respawn;

    public void PlayerTriggered(bool left, Collider2D collider) {
        if(left) _left = true;
        else _right = true;

        callbacks.Add(collider);

        if(_left && _right) {
            
            cameraChange.swap = true;

            callbacks.ForEach(n => n.isTrigger = false);
            respawn.PlayerWon(this);
            callbacks.Clear();
        }
    }


}
