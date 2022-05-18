using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public float totalTime;
    public bool completion = false;
    public string feedback;

    public List<bool> questions = new List<bool>();
    public List<bool> requiredHelp = new List<bool>();
    public List<float> timeSpent = new List<float>();

    public IEnumerator GameTimer()
    {
        while (DataManager.instance.gameState == TrackState.Tracking)
        {
            totalTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator QuestionTimer(int index)
    {
        while (DataManager.instance.questionState == TrackState.Tracking)
        {
            timeSpent[index] += Time.deltaTime;
            yield return null;
        }
    }
}
