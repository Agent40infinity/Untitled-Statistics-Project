using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public string identity;
    public List<string> queue = new List<string>();
    public int queueIndex = 0;
    public QuestionState lastState;
    public static bool queueWaiting = true;

    public DialogueController dialogueController;

    public void Awake()
    {
        dialogueController = GameObject.FindWithTag("Dialogue").GetComponent<DialogueController>();
        LoadLevel();
        LoadQuestion("Prelude", QuestionState.Dialogue);
    }

    public void Update()
    {
        switch (queueWaiting)
        { 
            case false:
                QueueUpdate();
                break;
        }
    }

    public void QueueUpdate()
    {
        int questionIndex = (int)lastState + 1;
        QuestionState state = (QuestionState)System.Enum.GetValues(typeof(QuestionState)).GetValue(questionIndex);

        if (DialogueData.currentlyLoaded.levelData[queue[queueIndex]].ContainsKey(state.ToString()))
        {
            LoadQuestion(queue[queueIndex], state);
        }
        else if (queueIndex < queue.Count)
        {
            queueIndex++;
            LoadQuestion(queue[queueIndex], QuestionState.Dialogue);
            Debug.Log(queueIndex);
        }
        else
        { 
            //Load next level
        }
    }

    public void LoadLevel()
    {
        DialogueLoading.LoadDialogue(identity);

        for (int i = 0; i < DialogueData.currentlyLoaded.levelData.Count; i++)
        {
            queue.Add(DialogueData.currentlyLoaded.levelData.ElementAt(i).Key);
            Debug.Log(queue[i]);
        }
    }

    public void LoadQuestion(string question, QuestionState state)
    {
        dialogueController.QuestionSetup(question, state);
        lastState = state;
        queueWaiting = true;
    }
}