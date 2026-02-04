using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using Saber.Base;
using UnityEngine.UI;


public class SceneManager : Singleton<SceneManager>
{
    public SceneFader sceneFaderPrefab;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void TransitionToDestination(SceneItems item)
    {
        string sceneName = Scenes.GetSceneName(item);

        StartCoroutine(TransitTo(sceneName));
    }

    IEnumerator TransitTo(string sceneName)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != sceneName)
        {
            SceneFader fader = Instantiate(sceneFaderPrefab);

            yield return StartCoroutine(fader.FadeOut(0.3f));
            yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            yield return StartCoroutine(fader.FadeIn(0.75f));
            yield break;
        }
        else
        {
            yield return null;
        }
    }

}

/*
 *
public class SceneManager : Singleton<SceneManager>
{
    

    public Image fadeImage;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Single));
    }

    private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        fadeImage.gameObject.SetActive(true);
        yield return StartCoroutine(FadeIn());
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, mode);
        asyncLoad.allowSceneActivation = false;
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = new Color(0, 0, 0, 1);
        while (elapsed < duration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = endColor;
    }

    private IEnumerator FadeOut()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Color startColor = new Color(0, 0, 0, 1);
        Color endColor = new Color(0, 0, 0, 0);
        while (elapsed < duration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = endColor;
        fadeImage.gameObject.SetActive(false);
    }
}

*/
