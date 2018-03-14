using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fader
{
    public Image blackBackGround;
    public AnimationCurve fadeCurve;

    public Fader(Image _blackBackGround, AnimationCurve _fadeCurve)
    {
        blackBackGround = _blackBackGround;
        fadeCurve = _fadeCurve;
    }

    public IEnumerator FadeInObject(MaskableGraphic _object)
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            float a = fadeCurve.Evaluate(t);
            _object.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    public IEnumerator FadeOutObject(MaskableGraphic _object)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            float a = fadeCurve.Evaluate(t);
            _object.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    public IEnumerator FadeInLevel()
    {
        blackBackGround.gameObject.SetActive(true);

        float t = 1f;

        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            float a = fadeCurve.Evaluate(t);
            blackBackGround.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        blackBackGround.gameObject.SetActive(false);
    }

    public IEnumerator FadeOutLevel(string scene)
    {
        blackBackGround.gameObject.SetActive(true);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            float a = fadeCurve.Evaluate(t);
            blackBackGround.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        blackBackGround.gameObject.SetActive(false);
    }
}
