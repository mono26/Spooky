using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    [Header("Load Manager settings")]
    [SerializeField]
    private const float exitFadeDuration = 1.0f;
    [SerializeField]
    private const float loadDelay = 1.0f;
    [SerializeField]
    private static string loadingScreenSceneName = "LoadScreen";

    [Header("Components")]
    [SerializeField]
    private Text loadingText;
    [SerializeField]
    private Image loadingFill;

    private AsyncOperation loadOperation;
    private static string sceneToLoad;

    public static void LoadScene(string _sceneToLoad)
    {
        sceneToLoad = _sceneToLoad;
        Application.backgroundLoadingPriority = ThreadPriority.High;
        if (loadingScreenSceneName != null)
        {
            SceneManager.LoadScene(loadingScreenSceneName);
        }

        return;
    }

    private void Awake()
    {
        if (loadingText == null)
            loadingText = GameObject.Find("LoadingText").GetComponent<Text>();
        if (loadingFill == null)
            loadingFill = GameObject.Find("LoadingBar").GetComponentInChildren<Image>();

        return;
    }

    private void Start()
    {
        loadingText.text = "Loading...";
        if (sceneToLoad != "")
        {
            StartCoroutine(LoadLevel());
        }

        return;
    }

    public IEnumerator LoadLevel()
    {
        StartLoading();

        while(loadOperation.progress < 0.9f)
        {
            loadingFill.fillAmount = loadOperation.progress;
            yield return null;
        }

        loadingFill.fillAmount = 1f;

        yield return new WaitForSecondsRealtime(loadDelay);

        EventManager.TriggerEvent(new FadeEvent(FadeEventType.FadeOut));
        yield return new WaitForSecondsRealtime(exitFadeDuration);

        loadOperation.allowSceneActivation = true;

        yield break;
    }

    private void StartLoading()
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        loadOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
        loadOperation.allowSceneActivation = false;

        return;
    }
}
