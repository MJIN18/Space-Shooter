using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnSinglePlayerModeButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void OnCoOpModeButtonClicked()
    {
        SceneManager.LoadScene(2);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
