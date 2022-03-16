using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string identity;
    public QueueState queue;

    public DialogueController dialogueController;

    public void Awake()
    {
        dialogueController = GameObject.FindWithTag("Dialogue").GetComponent<DialogueController>();

        LoadLevel(identity);
        LoadQuestion("Prelude");
    }

    public void LoadLevel(string identity)
    {
        dialogueController.Setup(identity);
    }

    public void LoadQuestion(string question)
    {
        dialogueController.QuestionSetup(question);
    }
}

public enum QueueState
{ 
    Prelude,
    Dialogue,
    Questions, 
    Responses,
}