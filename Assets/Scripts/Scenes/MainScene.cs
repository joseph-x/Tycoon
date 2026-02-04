using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class MainScene : MonoBehaviour
{
    public Button newGameButton;
    public Button loadGameButton;
    public Button quitGameButton;

    // Start is called before the first frame update
    void Start()
    {
        newGameButton?.onClick.AddListener(OnNewGame);
        loadGameButton?.onClick.AddListener(OnLoadGame);
        quitGameButton?.onClick.AddListener(OnQuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnNewGame()
    {
        //SceneManager.Instance.TransitionToDestination(SceneItems.Sample);
        SceneManager.Instance.TransitionToDestination(SceneItems.Game);
    }

    private void OnLoadGame()
    {
        SceneManager.Instance.TransitionToDestination(SceneItems.Sample);
    }

    private void OnQuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
