using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public string identity;
    public List<string> queue = new List<string>();
    public int queueIndex = 0;
    public static QuestionState lastState;
    public static bool queueWaiting = true;

    public DialogueController dialogueController;
    public FadeController fade;

    public void Awake()
    {
        dialogueController = GameObject.FindWithTag("Dialogue").GetComponent<DialogueController>();
        fade = GameObject.FindWithTag("FadeController").GetComponent<FadeController>();
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

        Debug.Log(questionIndex);
        Debug.Log(questionIndex < System.Enum.GetValues(typeof(QuestionState)).Length);
        if (questionIndex < System.Enum.GetValues(typeof(QuestionState)).Length)
        {
            QuestionState state = (QuestionState)System.Enum.GetValues(typeof(QuestionState)).GetValue(questionIndex);

            if (DialogueData.currentlyLoaded.levelData[FieldManager.GetState][queue[queueIndex]].ContainsKey(state.ToString()))
            {
                LoadQuestion(queue[queueIndex], state);
            }
            else
            {
                queueIndex++;
                LoadQuestion(queue[queueIndex], QuestionState.Dialogue);
            }
        }
        else if (queueIndex < queue.Count - 1)
        {
            queueIndex++;
            LoadQuestion(queue[queueIndex], QuestionState.Dialogue);
        }
        else
        {
            FieldManager.Completed = FieldManager.State;
            queueWaiting = true;
            StartCoroutine(NextLevel());
        }
    }

    public IEnumerator NextLevel()
    {
        yield return fade.FadeOut();
        GameManager.instance.SwapLevel();
    }

    public void LoadLevel()
    {
        DialogueLoading.LoadDialogue(identity);

        for (int i = 0; i < DialogueData.currentlyLoaded.levelData[FieldManager.GetState].Count; i++)
        {
            queue.Add(DialogueData.currentlyLoaded.levelData[FieldManager.GetState].ElementAt(i).Key);
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