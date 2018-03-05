using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private ScreenManager screenManager;

    public void PlayGame()
    {
        screenManager.StartCoroutine(screenManager.LoadLevel("Level1"));
    }

    public void Credits()
    {
        screenManager.StartCoroutine(screenManager.LoadLevel("Credits"));
    }

    public void GoToMainMenu()
    {
        screenManager.StartCoroutine(screenManager.LoadLevel("Main Menu"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
