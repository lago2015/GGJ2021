using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed;
    public float JumpForce;
    public LayerMask FloorMask;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _anim;
    private float _inputHorizontal;
    private bool _isJumping;
    private bool _isGrounded;
    private RaycastHit2D _ray;

    private const string jumpAnimName = "IsJumping", moveAnimName = "MoveSpeed";
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        _inputHorizontal = Input.GetAxis("Horizontal");
        _isJumping = Input.GetButtonDown("Jump");
        
        if(_inputHorizontal != 0)
        {
            _spriteRenderer.flipX = _inputHorizontal < 0;
        }
        if(_isJumping)
        {
            _rb.AddForce(transform.up * JumpForce);
            _anim.SetBool(jumpAnimName,true);
            _isGrounded = false;
        }
        
        transform.position += new Vector3(_inputHorizontal, 0, 0) * Time.deltaTime * MoveSpeed;
        if(_inputHorizontal < 0)
        {
            _inputHorizontal *= -1;
        }
        
        _anim.SetFloat(moveAnimName,_inputHorizontal);

        _ray = Physics2D.Raycast(transform.position, -transform.up, 5, FloorMask);
        if(_ray != null && !_isJumping && !_isGrounded)
        {
            _isGrounded = true;
            _anim.SetBool(jumpAnimName,false);
        }
    }
}
