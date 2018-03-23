using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LoadManager
{
    [SerializeField]
    private float minLoadDuration = 3f;
    private AsyncOperation loadOperation;

    public IEnumerator LoadLevel(int _levelIndex)
    {
        var startLoad = Time.unscaledTime;

        yield return SceneManager.LoadSceneAsync("LoadScene");

        GameManager.Instance.SceneFader.FadeInLevel();

        StartLoading(_levelIndex);

        while(!IsDoneLoading())
        {
            yield return 0;
        }

        yield return new WaitForSecondsRealtime(minLoadDuration);
    }

    private void StartLoading(int _levelIndex)
    {
        Time.timeScale = 0f;
        Application.backgroundLoadingPriority = ThreadPriority.Normal;
        loadOperation = SceneManager.LoadSceneAsync(_levelIndex, LoadSceneMode.Additive);
    }

    private bool IsDoneLoading()
    {
        return (loadOperation.isDone || loadOperation.progress >= 0.9f);
    }

    public IEnumerator FinishLoading()
    {
        Time.timeScale = 1f;
        GameManager.Instance.SceneFader.FadeOutLevel();

        yield return SceneManager.UnloadSceneAsync("LoadScene");

        GameManager.Instance.SceneFader.FadeInLevel();
    }
}
