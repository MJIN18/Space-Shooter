using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    private float _speedMultiplier = 2;
    [SerializeField] private float _xRange = 11.3f;
    [SerializeField] private float _yBound = 3.8f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleLaserPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _rightEngine, _leftEngine;
    [SerializeField] private AudioClip _laserShotAudio;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _fireRate = 0.3f;
    [SerializeField] private float _nextFire = -1f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private UIManager _uiManager;
    [SerializeField] private bool _isTripleShotActive = false;
    // [SerializeField] private bool _isSpeedPowerUpActive = false;
    [SerializeField] private bool _isShieldPowerUpActive = false;

    public bool _isPlayerOne = false;
    public bool _isPlayerTwo = false;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();


        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }
        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is NULL.");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }
        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on the player is NULL.");
        }
        if (!_gameManager.isCoOpModeActive)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Player Movement
        PlayerMovement();

        if (_gameManager.isCoOpModeActive)
        {
            if (_isPlayerOne && Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
            {
                FireLaser();
            }
            if (_isPlayerTwo && Input.GetKeyDown(KeyCode.RightShift) && Time.time > _nextFire)
            {
                FireLaser();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
            {
                FireLaser();
            }
        }

    }

    void PlayerMovement()
    {
        if (!_gameManager.isCoOpModeActive)
        {
            float horizontalInput;
            float verticalInput;
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
            //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            if (_isPlayerOne)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    transform.Translate(Vector3.up * _speed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.Translate(Vector3.right * _speed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    transform.Translate(Vector3.left * _speed * Time.deltaTime);
                }
            }
            else if (_isPlayerTwo)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(Vector3.up * _speed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    transform.Translate(Vector3.right * _speed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.Translate(Vector3.left * _speed * Time.deltaTime);
                }
            }
        }

        // Player Movement Constraints
        MovementConstraints();
    }

    void MovementConstraints()
    {
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        else if (transform.position.y <= -_yBound)
        {
            transform.position = new Vector3(transform.position.x, -_yBound, transform.position.z);
        }

        //transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), transform.position.z);

        if (transform.position.x > _xRange)
        {
            transform.position = new Vector3(-_xRange, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -_xRange)
        {
            transform.position = new Vector3(_xRange, transform.position.y, transform.position.z);
        }
    }

    void FireLaser()
    {
        Vector3 offset;
        _nextFire = Time.time + _fireRate;
        offset = transform.position + new Vector3(0, 1.05f, 0);

        if (_isTripleShotActive)
        {
            Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, offset, Quaternion.identity);
        }
        _audioSource.clip = _laserShotAudio;
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldPowerUpActive)
        {
            _isShieldPowerUpActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        else
        {
            _lives -= 1;

            _uiManager.UpdateLives(_lives);

            switch (_lives)
            {
                case 2:
                    _rightEngine.SetActive(true);
                    break;
                case 1:
                    _leftEngine.SetActive(true);
                    break;
                case 0:
                    Destroy(gameObject);
                    GameOver();
                    _spawnManager.OnPlayerDeath();
                    break;
            }
        }
    }
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    public void SpeedPowerUpActive()
    {
        // _isSpeedPowerUpActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    public void ShieldsActive()
    {
        _isShieldPowerUpActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int score)
    {
        _score += score;
        _uiManager.UpdateScore(_score);
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _speed /= _speedMultiplier;
        // _isSpeedPowerUpActive = false;
    }

    private void GameOver()
    {
        _uiManager.GameOverSequence();
        if (_gameManager.isCoOpModeActive == false)
        {
            _uiManager.CheckForBestScore();
        }
    }
}
