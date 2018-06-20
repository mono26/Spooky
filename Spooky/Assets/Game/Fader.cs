using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum FadeEventType { FadeOut, FadeIn}

public class FadeEvent : SpookyCrowEvent
{
    public FadeEventType type;

    public FadeEvent(FadeEventType _type)
    {
        type = _type;
    }
}

public class Fader : MonoBehaviour, EventHandler<FadeEvent>
{
    private Image blackBackGround;
    [SerializeField]
    private AnimationCurve fadeCurve;

    private void Awake()
    {
        blackBackGround = GetComponent<Image>();
    }

    private void OnEnable()
    {
        EventManager.AddListener<FadeEvent>(this);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<FadeEvent>(this);
    }

    private IEnumerator FadeOutLevel()
    {
        blackBackGround.gameObject.SetActive(true);

        float t = 1f;

        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            float a = fadeCurve.Evaluate(t);
            blackBackGround.color = new Color(0f, 0f, 0f, a);
            yield return null;
        }

        blackBackGround.gameObject.SetActive(false);
    }

    private IEnumerator FadeInLevel()
    {
        blackBackGround.gameObject.SetActive(true);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            float a = fadeCurve.Evaluate(t);
            blackBackGround.color = new Color(0f, 0f, 0f, a);
            yield return null;
        }

        blackBackGround.gameObject.SetActive(false);
    }

    public void OnEvent(FadeEvent _fadeEvent)
    {
        if(_fadeEvent.type == FadeEventType.FadeOut)
            StartCoroutine(FadeOutLevel());
        else if (_fadeEvent.type == FadeEventType.FadeIn)
            StartCoroutine(FadeInLevel());
        return;
    }
}
