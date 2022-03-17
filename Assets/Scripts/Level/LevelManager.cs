using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public string identity;
    public Dictionary<string, List<string>> queue = new Dictionary<string, List<string>>();
    public static bool queueWaiting = true;

    public DialogueController dialogueController;

    public void Awake()
    {
        dialogueController = GameObject.FindWithTag("Dialogue").GetComponent<DialogueController>();

        LoadLevel();
    }

    public void Update()
    {
        switch (queueWaiting)
        { 
            case true:
                LoadQuestion("Prelude", QuestionState.Dialogue);
                break;
            case false:

                break;
        }
    }

    public void LoadLevel()
    {
        DialogueLoading.LoadDialogue(identity);

        for (int i = 0; i < DialogueData.currentlyLoaded.levelData.Count; i++)
        {
            for (int j = 0; j < DialogueData.currentlyLoaded.levelData.Count; j++)
            {

            }
            /*queue.Add(DialogueData.currentlyLoaded.levelData.ElementAt(i).Key, DialogueData.currentlyLoaded.levelData.ElementAt(i).Key.ElementAt().Value);*/
        }
    }

    public void LoadQuestion(string question, QuestionState state)
    {
        dialogueController.QuestionSetup(question, state);
        queueWaiting = true;
    }
}