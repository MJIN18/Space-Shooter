using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private int _powerupID;
    private float _lowerBound = -4.5f;
    private List<PlayerController> _player;
    private GameManager _gameManager;
    [SerializeField] private AudioClip _powerupClip;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _player = new List<PlayerController>();

        if (_gameManager.isCoOpModeActive)
        {
            _player.Add(GameObject.Find("Player One").GetComponent<PlayerController>());
            _player.Add(GameObject.Find("Player Two").GetComponent<PlayerController>());
        }
        else
        {
            _player.Add(GameObject.Find("Player").GetComponent<PlayerController>());
        }

        if (_player.Count == 0)
        {
            Debug.LogError("No players found.");
        }
        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
        DestroyOutOfBounds();
    }

    void DestroyOutOfBounds()
    {
        if (transform.position.y < _lowerBound)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_player != null)
            {
                if (_gameManager.isCoOpModeActive)
                {
                    if (other.gameObject.name == "Player One")
                    {
                        ActivatePowerups(0);
                    }
                    else if (other.gameObject.name == "Player Two")
                    {
                        ActivatePowerups(1);
                    }
                }
                else
                {
                    ActivatePowerups(0);
                }
            }
            AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
            Destroy(gameObject);
        }
    }
    void ActivatePowerups(int playerNum)
    {
        switch (_powerupID)
        {
            case 0:
                _player[playerNum].TripleShotActive();
                break;
            case 1:
                _player[playerNum].SpeedPowerUpActive();
                break;
            case 2:
                _player[playerNum].ShieldsActive();
                break;
            default:
                Debug.Log("Default Value");
                break;
        }
    }
}


