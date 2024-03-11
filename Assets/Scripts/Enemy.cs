using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private float _lowerBound = -6.0f;
    [SerializeField] private float _spawnPosition = 6.0f;
    [SerializeField] private float _xRange = 9.0f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 3.0f;
    [SerializeField] private float _nextFire = -1f;
    private PlayerController player;
    private Animator _animator;
    [SerializeField] private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        _audioSource = GetComponent<AudioSource>();

        if(player == null)
        {
            Debug.LogError("Player is NULL.");
        }
        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is NULL.");
        }
        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source on the enmy is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if(Time.deltaTime > _nextFire)
        {
            FireLaser();
        }
    }

    void FireLaser()
    {
        _fireRate = Random.Range(3.0f, 7.0f);
        _nextFire = Time.time + _fireRate;
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        //Debug.Break();
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();   
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }

    void Movement()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < _lowerBound)
        {
            float xRandomPosition = Random.Range(-_xRange, _xRange);
            transform.position = new Vector3(xRandomPosition, _spawnPosition, 0.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(player != null)
            {
                player.Damage();
                _speed = 0;
                _animator.SetTrigger("OnEnemyDeath");
                _audioSource.Play();
                Destroy(GetComponent<Collider2D>());
                Destroy(gameObject, 2.5f);   
            }      
        }

        if (other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (player != null)
            {
                player.AddScore(10);
            }
            _speed = 0;
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.5f);   
        }
    }
}
