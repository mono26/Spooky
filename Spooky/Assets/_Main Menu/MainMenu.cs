using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string mainMenuScene;
    public string playLevelScene;
    public string creditsScene;

    public void GoToMainMenu()
    {
        if (mainMenuScene != "")
        {
            LoadManager.LoadScene(mainMenuScene);
            return;
        }
        else return;
    }

    public void PlayGame()
    {
        if (mainMenuScene != "")
        {
            LoadManager.LoadScene(playLevelScene);
            return;
        }
        else return;
    }

    public void Credits()
    {
        if (mainMenuScene != "")
        {
            LoadManager.LoadScene(creditsScene);
            return;
        }
        else return;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
