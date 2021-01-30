using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates{Falling,Jumping,Grounded}
public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    private PlayerStates _currentState;
    
    private const string _jumpAnimParam = "IsJumping";
    private const string _moveAnimParam = "MoveSpeed";
    private const string _groundedAnimParam = "IsGrounded";
    private const string _fallingAnimParam = "IsFalling";
    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        _currentState = PlayerStates.Grounded;
    }

    public void SetMoveSpeed(bool isActive)
    {
        _anim.SetFloat(_moveAnimParam,isActive ? 1 : 0);
    }
    
    public void SetAnimation(PlayerStates newState)
    {
        _currentState = newState;
        switch(_currentState)
        {
            case PlayerStates.Falling:
                _anim.SetBool(_fallingAnimParam,true);
                _anim.SetBool(_jumpAnimParam, false);
                _anim.SetBool(_groundedAnimParam,false);
                break;
            case PlayerStates.Jumping:
                _anim.SetBool(_jumpAnimParam,true);
                _anim.SetBool(_groundedAnimParam,false);
                _anim.SetBool(_fallingAnimParam,false);
                break;
            case PlayerStates.Grounded:
                _anim.SetBool(_groundedAnimParam,true);
                _anim.SetBool(_jumpAnimParam,false);
                _anim.SetBool(_fallingAnimParam,false);
                break;
        }
    }
}
