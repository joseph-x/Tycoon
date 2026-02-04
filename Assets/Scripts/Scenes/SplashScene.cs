using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Transit", 2f);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void Transit()
    {
        SceneManager.Instance.TransitionToDestination(SceneItems.Main);
    }
}
