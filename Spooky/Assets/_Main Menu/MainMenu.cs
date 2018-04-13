using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string mainMenuScene;
    public string playLevelScene;
    public string creditsScene;

    public void GoToMainMenu()
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.LoadLevel(mainMenuScene));
    }

    public void PlayGame()
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.LoadLevel(playLevelScene));
    }

    public void Credits()
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.LoadLevel(playLevelScene));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
