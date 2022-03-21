using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public GameObject dialogueSelection;
    public GameObject dialogueBox;
    public Dialogue dialogue;
    public List<TextMeshProUGUI> options = new List<TextMeshProUGUI>();
    public string[] modifiers =
    {
        "$c"
    };

    public string currentQuestion;

    public void QuestionSetup(string question, QuestionState state)
    {
        currentQuestion = question;

        switch (state)
        {
            case QuestionState.Questions:
                StartCoroutine(DialogueSelection());
                break;
            case QuestionState.Dialogue: case QuestionState.Responses:
                DisplayDialogue(QuestionState.Dialogue, 0);
                break;
        }
    }

    public IEnumerator DialogueSelection()
    {
        dialogueSelection.SetActive(true);

        string[] decompiledTitle = new string[options.Count];

        for (int i = 0; i < decompiledTitle.Length; i++)
        {
            //Debug.Log(DialogueData.currentlyLoaded[question["Questions"[]]]);
            decompiledTitle[i] = DialogueData.currentlyLoaded.levelData[currentQuestion]["Questions"][(i + 1).ToString()];
        }

        for (int i = 0; i < options.Count; i++)
        {
            options[i].text = decompiledTitle[i];
        }

        yield return null;
    }

    public void DisplayDialogue(QuestionState state, int optionIndex)
    {
        DialogueActivation();

        Dictionary<string, string> loadedDialogue = loadedDialogue = DialogueData.currentlyLoaded.levelData[currentQuestion][state.ToString()];

        switch (state)
        {
            case QuestionState.Responses:
                if (int.Parse(DialogueData.currentlyLoaded.answers[currentQuestion]) == optionIndex)
                {
                    loadedDialogue.Remove("Incorrect");
                }
                else
                {
                    loadedDialogue.Remove("Correct");
                }
               /* if (loadedDialogue["Correct"].Contains(modifiers[0]))
                {
                    string[] processedDialogue = loadedDialogue["Correct"].Split(new string[] { modifiers[0] }, System.StringSplitOptions.None);
                    loadedDialogue["Correct"] = processedDialogue[1];

                    if (int.Parse(processedDialogue[0]) == optionIndex)
                    { 
                        
                    }
                    
                }*/
                break;
        }

        dialogue.dialogue = loadedDialogue;

        dialogue.dialogueState = DialogueState.Load;
    }

    public void OptionSelection(int optionIndex)
    {
        DisplayDialogue(QuestionState.Responses, optionIndex);
    }

    public void DialogueActivation()
    {
        dialogueSelection.SetActive(false);
        dialogueBox.SetActive(true);
    }
}

public enum QuestionState
{ 
    Dialogue,
    Questions,
    Responses
}