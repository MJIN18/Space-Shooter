using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private GameManager _gameManager;
    private PlayerController _player;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _animator = GetComponent<Animator>();
        _player = GetComponent<PlayerController>();

        if (_gameManager == null)
        {
            Debug.LogError("The Game Manager is NULL.");
        }
        if (_animator == null)
        {
            Debug.LogError("The Animator on the player is NULL.");
        }
        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAnim();
    }

    void PlayerAnim()
    {
        if (!_gameManager.isCoOpModeActive)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                _animator.SetBool("Turn_Right", true);
                _animator.SetBool("Turn_Left", false);
            }
            else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                _animator.SetBool("Turn_Right", false);
                _animator.SetBool("Turn_Left", false);
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _animator.SetBool("Turn_Right", false);
                _animator.SetBool("Turn_Left", true);
            }
            else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                _animator.SetBool("Turn_Right", false);
                _animator.SetBool("Turn_Left", false);
            }
        }
        else if(_gameManager.isCoOpModeActive)
        {
            if (_player._isPlayerOne)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    _animator.SetBool("Turn_Right", true);
                    _animator.SetBool("Turn_Left", false);
                }
                else if (Input.GetKeyUp(KeyCode.D))
                {
                    _animator.SetBool("Turn_Right", false);
                    _animator.SetBool("Turn_Left", false);
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    _animator.SetBool("Turn_Right", false);
                    _animator.SetBool("Turn_Left", true);
                }
                else if (Input.GetKeyUp(KeyCode.A))
                {
                    _animator.SetBool("Turn_Right", false);
                    _animator.SetBool("Turn_Left", false);
                }
            }
            if (_player._isPlayerTwo)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    _animator.SetBool("Turn_Right", true);
                    _animator.SetBool("Turn_Left", false);
                }
                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    _animator.SetBool("Turn_Right", false);
                    _animator.SetBool("Turn_Left", false);
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    _animator.SetBool("Turn_Right", false);
                    _animator.SetBool("Turn_Left", true);
                }
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    _animator.SetBool("Turn_Right", false);
                    _animator.SetBool("Turn_Left", false);
                }
            }
        }
    }
}
