using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saber.Base;

public class SaveManager : Singleton<SaveManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }


}
