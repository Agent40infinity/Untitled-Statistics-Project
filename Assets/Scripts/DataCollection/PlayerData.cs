using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public float totalTime;
    public bool completion = false;
    public string feedback;

    public Dictionary<string, bool> questions = new Dictionary<string, bool>();
    public Dictionary<string, bool> requiredHelp = new Dictionary<string, bool>();
    public Dictionary<string, float> timeSpent = new Dictionary<string, float>();

    public IEnumerator GameTimer()
    {
        while (DataManager.instance.gameState == TrackState.Tracking)
        {
            totalTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator QuestionTimer(string index)
    {
        while (DataManager.instance.questionState == TrackState.Tracking)
        {
            timeSpent[index] += Time.deltaTime;
            yield return null;
        }
    }
}
