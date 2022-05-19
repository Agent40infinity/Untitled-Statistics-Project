using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultDisplay : MonoBehaviour
{
    public TMP_InputField results;


    public void Submit()
    {
        DataManager.playerData.feedback = results.text;
        DataManager.instance.SaveData();
    }
}
