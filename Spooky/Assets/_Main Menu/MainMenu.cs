using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.LoadLevel(0));
    }

    public void PlayGame()
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.LoadLevel(2));
    }

    public void Credits()
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.LoadLevel(3));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
