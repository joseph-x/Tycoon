using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneItems
{
    None,
    Splash,
    Main,
    Game,
    Sample
}

public static class Scenes
{
    public static string GetSceneName(SceneItems item)
    {
        string name = null;

        switch (item)
        {
            case SceneItems.None:
                name = string.Empty;
                break;
            case SceneItems.Main:
                name = "MainScene";
                break;
            case SceneItems.Game:
                name = "GameScene";
                break;
            case SceneItems.Splash:
                name = "SplashScene";
                break;
            case SceneItems.Sample:
                name = "SampleScene";
                break;
        }

        return name;
    }
}
