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
    private float _inputSpeed = 1;
    private float _isJumpingAxis;
    private float _floorCheckDistance = 0.25f;
    private bool _isJumping;
    private bool _isGrounded;
    private bool _isShooting;
    private bool _ableToShoot;
    private bool _isActive;

    private PlayerAnimation _actor;
    private RaycastHit2D _ray;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private PlayerRespawn _playerRespawn;
    private int _specialDirection;    //meant to give either 1 or -1 to give the opposite robot an inverted behavior
    
    private const string _jump1ButtonName = "Jump";
    private const string _shoot1ButtonName = "Fire1";
    private const string _shoot2ButtonName = "Fire2";
    
    public bool IsActive
    {
        get { return _isActive;}
        set
        {
            if(_actor) _actor.SetMoveSpeed(value); 
            _isActive = value;
        }
    }
    
    private void Awake()
    {
        _playerRespawn = FindObjectOfType<PlayerRespawn>();
        _rb = GetComponent<Rigidbody2D>();
        _actor = GetComponent<PlayerAnimation>();
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
        if(!IsActive)
        {
            _actor.SetMoveSpeed(false);
            return;
        }
        Movement();
        Jump();
        //Shoot();
        FloorAndFallingCheck();
        /*//Check velocity if to determine death
        if(_rb.velocity.magnitude <= 0)
        {
            _playerRespawn.GameOver();
        }*/
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
        //Jumping Check
        _isJumpingAxis = Input.GetAxis(_jump1ButtonName);
        if(_isJumpingAxis > 0 && _isGrounded && !_isJumping)
        {
            _rb.AddForce(transform.up * JumpForce);
            _actor.SetAnimation(PlayerStates.Jumping);
            StartCoroutine(DelayToCheckFloor());
            _isJumping = true;
        }
        
    }

    private void FloorAndFallingCheck()
    {
        //Checking if player is falling
        if(_rb.velocity.y < 0)
        {
            _actor.SetAnimation(PlayerStates.Falling);
            _isJumping = true;
            _isGrounded = false;
        }
        //Floor Checking
        if(!_isGrounded && _isJumping)
        {
            _ray = Physics2D.Raycast(transform.position, -transform.up, _floorCheckDistance, FloorMask);
            //Debug.DrawRay(transform.position,-transform.up * _floorCheckDistance,Color.red,2);
            if(_ray.collider != null)
            {
                _isJumping = false;
                _actor.SetAnimation(PlayerStates.Grounded);
                StartCoroutine(DelayToEnableJump());
            }    
        }
    }

    IEnumerator DelayToEnableJump()
    {
        yield return new WaitForFixedUpdate();
        _isGrounded = true;
        
    }

    IEnumerator DelayToCheckFloor()
    {
        yield return new WaitForSeconds(0.25f);
        _isGrounded = false;
    }

    private void Movement()
    {
        //Movement
        transform.position += new Vector3(_inputSpeed, 0, 0) * Time.deltaTime * MoveSpeed * _specialDirection;
    }

    
}
