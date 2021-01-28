using System;
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
    public float FireRate;
    public float MoveSpeed;
    public float JumpForce;
    public LayerMask FloorMask;
    public Transform GunLeftTransform;
    public Transform GunRightTransform;
    public GameObject BulletPrefab;
    
    
    private float _cooldownTime;
    private float _inputHorizontal;
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
    private const string _jumpButtonName = "Jump";
    private const string _shootButtonName = "Fire1";
    private const string _horizontalAxisName = "Horizontal";
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _specialDirection = MovementInverted ? -1 : 1;
    }

    private void Update()
    {
        Movement();
        Shoot();
    }

    private void Shoot()
    {
        _isShooting = Input.GetButtonDown(_shootButtonName);
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

    private void Movement()
    {
        _inputHorizontal = Input.GetAxis(_horizontalAxisName);
        _isJumping = Input.GetButtonDown(_jumpButtonName);
        
        if(_inputHorizontal != 0)
        {
            _spriteRenderer.flipX = _inputHorizontal < 0;
        }
        if(_isJumping)
        {
            _rb.AddForce(transform.up * JumpForce);
            _anim.SetBool(_jumpAnimName,true);
            _isGrounded = false;
        }
        
        transform.position += new Vector3(_inputHorizontal, 0, 0) * Time.deltaTime * MoveSpeed * _specialDirection;
        var animMoveSpeed = _inputHorizontal;
        if(animMoveSpeed < 0)
        {
            animMoveSpeed *= -1;
        }
        
        _anim.SetFloat(_moveAnimName,animMoveSpeed);

        _ray = Physics2D.Raycast(transform.position, -transform.up, 5, FloorMask);
        if(_ray != null && !_isJumping && !_isGrounded)
        {
            _isGrounded = true;
            _anim.SetBool(_jumpAnimName,false);
        }
    }
}
