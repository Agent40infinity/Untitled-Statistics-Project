using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultDisplay : MonoBehaviour
{
    public TextMeshProUGUI score;

    public void Start()
    {
        string output = GameManager.playerEntry.score.ToString() + "/" + GameManager.playerEntry.maxScore.ToString();
        score.text = output;
    }
}
