using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public string identity;
    public int queueSize;
    public int queueIndex = 0;
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

        }
    }

    public void LoadLevel()
    {
        DialogueLoading.LoadDialogue(identity);
        queueSize = DialogueData.currentlyLoaded.levelData.Count;
    }

    public void LoadQuestion(string question, QuestionState state)
    {
        dialogueController.QuestionSetup(question, state);
        queueWaiting = true;
    }
}