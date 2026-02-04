using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public CanvasGroup inGameMenuCanvas;

    private bool isMenuOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isMenuOpened == false)
            {
                OpenInGameMenu();
            }
            else
            {
                CloseInGameMenu();
            }
        }
    }

    public void OpenInGameMenu()
    {
        inGameMenuCanvas.alpha = 1.0f;
        isMenuOpened = true;

    }

    public void CloseInGameMenu()
    {
        inGameMenuCanvas.alpha = 0.0f;
        isMenuOpened = false;
    }

    public void BackToMainMenu()
    {
        SceneManager.Instance.TransitionToDestination(SceneItems.Main);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
