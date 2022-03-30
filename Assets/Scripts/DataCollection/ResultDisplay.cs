using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultDisplay : MonoBehaviour
{
    public TextMeshProUGUI results;


    public void Start()
    {
        string output = GameManager.playerData.firstName + " " + GameManager.playerData.lastName + ": " + GameManager.playerData.playerID + "\n" + GameManager.playerData.score.ToString() + "/" + GameManager.playerData.maxScore.ToString();
        results.text = output;
    }
}
