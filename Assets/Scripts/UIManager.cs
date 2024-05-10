using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText, _bestScoreText;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _restartText;
    [SerializeField] private Image _livesImg;
    [SerializeField] private Sprite[] _liveSprites;
    private int _currentScore = 0, _bestScore;
    private GameManager _gameManager;

    private void Start()
    {
        _scoreText.text = "Score: " + 0;
        _bestScore = PlayerPrefs.GetInt("Best Score", 0);
        _bestScoreText.text = "Best Score: " + _bestScore;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
        
    }
    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
        _currentScore = score;
    }

    public void CheckForBestScore()
    {      
            if (_currentScore > _bestScore)
            {
                _bestScore = _currentScore;
                _bestScoreText.text = "Best Score: " + _bestScore;
                PlayerPrefs.SetInt("Best Score", _bestScore);
            }
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
    }

    public void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            if (_gameOverText.gameObject.activeInHierarchy)
            {
                _gameOverText.gameObject.SetActive(false);
            }
            else
            {
                _gameOverText.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void OnResumeButtonClicked()
    {
        _gameManager.ResumeGame();
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
