using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionCheck : MonoBehaviour
{
    public int MaxPositions;
    private List<Vector2> _previousPositions = new List<Vector2>();
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if(!_playerController.IsActive) return;
        
        //Remove the last position if the list gets full
        if(MaxPositions == _previousPositions.Count)
        {
            _previousPositions.Remove(_previousPositions[0]);
        }
        //Round to one decimal place for current position for more accurate comparisons
        var currentPosition = transform.position;
        currentPosition.x =(float) Math.Round(currentPosition.x,1);
        currentPosition.y = (float) Math.Round(currentPosition.y, 1);
        _previousPositions.Add(currentPosition);
        if(_previousPositions.Count > MaxPositions / 2)
        {
            var position = (Vector2) transform.position;
            position.x =(float) Math.Round(position.x,1);
            position.y = (float) Math.Round(position.y, 1);
            if(_previousPositions[0] == position)
            {
                //Game over
                DataManager.GetValue<PlayerRespawn>(DataKeys.PLAYER_RESPAWN).GameOver();
            }
        }
    }
}
