using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float MoveSpeed;
    public float LifeSpan;
    
    private Rigidbody2D _rigidbody;
    private float _lifeTimer;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(_lifeTimer > LifeSpan)
        {
            Destroy(gameObject);   
        }
        else
        {
            _lifeTimer += Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        _rigidbody.AddForce(transform.up * MoveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}
