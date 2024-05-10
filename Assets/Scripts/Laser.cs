using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
    private float _upperBound = 8.0f;
    private float _lowerBound = -6.0f;
    [SerializeField] private bool _isEnemyLaser = false;
    private PlayerController player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (player == null)
        {
            Debug.LogError("Player is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move Up
        if (_isEnemyLaser)
        {
            MoveDown();
        }
        else
        {
            MoveUp();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        DestroyOutOfBounds();
    }
    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        DestroyOutOfBounds();
    }

    private void DestroyOutOfBounds()
    {
            if ((!_isEnemyLaser && gameObject.transform.position.y > _upperBound) || (_isEnemyLaser && gameObject.transform.position.y < _lowerBound))
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(gameObject);
            }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && _isEnemyLaser)
        {
            if (player != null)
            {
                player.Damage();
                Destroy(transform.parent.gameObject);
            }
        }
    }

}