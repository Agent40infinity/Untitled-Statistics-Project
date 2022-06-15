using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public string identity;
    public int questionIndex;
    public static bool queueWaiting = true;

    public DialogueController dialogueController;
    public FadeController fade;

    public void Awake()
    {
        questionIndex = 0;
        dialogueController = GameObject.FindWithTag("Dialogue").GetComponent<DialogueController>();
        fade = GameObject.FindWithTag("FadeController").GetComponent<FadeController>();
        StartCoroutine(LoadLevel());
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
        if (DialogueData.currentlyLoaded.levelData[FieldManager.State][dialogueController.currentQuestion.name].next == "end")
        {
            StartCoroutine(NextLevel());
            return;
        }

        if (dialogueController.isNext)
        {
            LoadQuestion(DialogueData.currentlyLoaded.levelData[FieldManager.State][dialogueController.currentQuestion.next]);
        }
        else
        {
            LoadQuestion(DialogueData.currentlyLoaded.levelData[FieldManager.State][dialogueController.currentQuestion.alternative]); 
        }

    }

    public IEnumerator NextLevel()
    {
        queueWaiting = true;
        yield return fade.FadeOut(name);
        GameManager.instance.SwapLevel();
    }

    public IEnumerator LoadLevel()
    {
        yield return DialogueLoading.instance.LoadDialogue(identity);

        if (!DialogueLoading.instance.HasLoaded())
        {
            yield return null;
        }

        LoadQuestion(DialogueData.currentlyLoaded.levelData[FieldManager.State].ElementAt(questionIndex).Value);
    }

    public void LoadQuestion(Question question)
    {
        dialogueController.QuestionSetup(question);
        dialogueController.isNext = true;
        queueWaiting = true;
    }
}