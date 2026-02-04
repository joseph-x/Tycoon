using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class SceneFader : MonoBehaviour
{
    public float fadeInDuration;
    public float fadeOutDuration;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator FadeOutIn()
    {
        yield return FadeOut(fadeOutDuration);
        yield return FadeIn(fadeInDuration);
    }

    public IEnumerator FadeOut(float duration)
    {
        // 0 -> 1
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / duration;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float duration)
    {
        // 1 -> 0
        while (canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime / duration;
            yield return null;
        }

        Destroy(gameObject);
    }
}
