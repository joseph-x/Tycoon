using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saber.Base;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            Debug.Log(playerStats.CurrentCurrency);
        }
    }
}
