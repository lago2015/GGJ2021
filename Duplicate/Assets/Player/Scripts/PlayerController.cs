﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class PlayerController : MonoBehaviour
{
    [Header("Specific for inverted Robot")]
    public bool MovementInverted;
    public bool IsActive;
    public float FireRate;
    public float MoveSpeed;
    public float JumpForce;
    public LayerMask FloorMask;
    public Transform GunLeftTransform;
    public Transform GunRightTransform;
    public GameObject BulletPrefab;

    private float _cooldownTime;
    private float _inputSpeed = 1;
    private float _isJumpingAxis;
    private float _floorCheckDistance = 0.5f;
    private bool _isJumping;
    private bool _isGrounded;
    private bool _isShooting;
    private bool _ableToShoot;
    private RaycastHit2D _ray;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _anim;
    private int _specialDirection;    //meant to give either 1 or -1 to give the opposite robot an inverted behavior
    private const string _jumpAnimName = "IsJumping";
    private const string _moveAnimName = "MoveSpeed";
    private const string _jump1ButtonName = "Jump";
    private const string _shoot1ButtonName = "Fire1";
    private const string _shoot2ButtonName = "Fire2";
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _specialDirection = MovementInverted ? -1 : 1;
        
    }

    private void OnEnable()
    {
        _ableToShoot = true;
        _isGrounded = true;
        _isJumping = false;
        _isShooting = false;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Submit"))
        {
            IsActive = true;
        }
        if(!IsActive)
        {
            _anim.SetFloat(_moveAnimName, 0);
            return;
        }
        Movement();
        Jump();
        Shoot();
    }

    private void Shoot()
    {
        _isShooting = Input.GetButtonDown(MovementInverted ? _shoot2ButtonName : _shoot1ButtonName);
        if(_isShooting && _ableToShoot)
        {
            _ableToShoot = false;
            _cooldownTime = Time.time + FireRate;
            var spawnPoint = _spriteRenderer.flipX ? GunLeftTransform : GunRightTransform;
            Instantiate(BulletPrefab, spawnPoint.position,spawnPoint.rotation);
        }
        else if(Time.time > _cooldownTime)
        {
            _ableToShoot = true;
        }
    }

    private void Jump()
    {
        _isJumpingAxis = Input.GetAxis(_jump1ButtonName);
        if(_isJumpingAxis > 0 && _isGrounded && !_isJumping)
        {
            _rb.AddForce(transform.up * JumpForce);
            _anim.SetBool(_jumpAnimName,true);
            StartCoroutine(DelayToCheckFloor());
            _isJumping = true;
        }
        
        //Floor Checking
        if(!_isGrounded && _isJumping)
        {
            _ray = Physics2D.Raycast(transform.position, -transform.up, _floorCheckDistance, FloorMask);
            //Debug.DrawRay(transform.position,-transform.up * _floorCheckDistance,Color.red,2);
            if(_ray.collider != null)
            {
                _isGrounded = true;
                _isJumping = false;
                _anim.SetBool(_jumpAnimName,false);
            }    
        }
    }

    IEnumerator DelayToCheckFloor()
    {
        yield return new WaitForSeconds(0.25f);
        _isGrounded = false;
    }

    private void Movement()
    {
        _anim.SetFloat(_moveAnimName,_inputSpeed);
        //Movement
        transform.position += new Vector3(_inputSpeed, 0, 0) * Time.deltaTime * MoveSpeed * _specialDirection;
    }
}
