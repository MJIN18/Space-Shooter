using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isCoOpModeActive = false;
    [SerializeField] private bool _isGameOver;
    [SerializeField] private GameObject _pausePanel;
    private Animator _pauseMenuAnimator;

    private void Start()
    {
        _pauseMenuAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        if(_pauseMenuAnimator == null)
        {
            Debug.LogError("The Animator on the Pause Menu Panel is NULL.");
        }

        _pauseMenuAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            if (isCoOpModeActive)
            {
                SceneManager.LoadScene(2);
            }
            else
            {
                SceneManager.LoadScene(1); // Current Game Scene 
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Scene scene = SceneManager.GetActiveScene();
            if(scene.buildIndex == 1 || scene.buildIndex == 2)
            {
                SceneManager.LoadScene(0);
            }
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P) && !_isGameOver)
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        _pausePanel.SetActive(true);
        _pauseMenuAnimator.SetBool("isPaused", true);
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void ResumeGame()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
