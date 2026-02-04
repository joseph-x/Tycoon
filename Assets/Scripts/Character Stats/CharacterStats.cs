using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO templateData;
    public CharacterData_SO characterData;

    #region Data_SO
    public int CurrentCurrency
    {
        get { return characterData.currency; }
        set { characterData.currency = value; }
    }
    #endregion

    private void Awake()
    {
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }
    }
}
