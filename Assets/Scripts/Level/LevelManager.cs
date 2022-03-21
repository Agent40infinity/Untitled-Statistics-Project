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
                //QueueUpdate();
                break;
        }
    }

    /*public void QueueUpdate()
    {
        if (DialogueData.currentlyLoaded.levelData[queue[queueIndex]])
        queue[queueIndex] (PerkType)System.Enum.GetValues(typeof(PerkType)).GetValue(index)
    }*/

    public void LoadLevel()
    {
        DialogueLoading.LoadDialogue(identity);

        for (int i = 0; i < DialogueData.currentlyLoaded.levelData.Count; i++)
        {
            queue.Add(DialogueData.currentlyLoaded.levelData.ElementAt(i).Key);
        }
    }

    public void LoadQuestion(string question, QuestionState state)
    {
        dialogueController.QuestionSetup(question, state);
        lastState = state;
        queueWaiting = true;
    }
}