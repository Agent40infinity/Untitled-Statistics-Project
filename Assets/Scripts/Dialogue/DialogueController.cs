using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DialogueController : MonoBehaviour
{
    public GameObject dialogueSelection;
    public GameObject dialogueBox;
    public Dialogue dialogue;
    public List<TextMeshProUGUI> options = new List<TextMeshProUGUI>();
    public string[] modifiers =
    {
        "$T",
        "$CE"
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

        Dictionary<string, string> loadedDialogue = DialogueData.currentlyLoaded.levelData[currentQuestion][state.ToString()];

        switch (state)
        {
            case QuestionState.Responses:
                string answer;

                if (DialogueData.currentlyLoaded.answers[currentQuestion] == optionIndex)
                {
                    answer = "Incorrect";
                }
                else
                {
                    answer = "Correct";
                }

                for (int i = 0; i < loadedDialogue.Count; i++)
                {
                    if (loadedDialogue.ElementAt(i).Key.Contains(answer))
                    {
                        loadedDialogue.Remove(loadedDialogue.ElementAt(i).Key);
                    }
                }

                LevelManager.lastState = QuestionState.Responses;
                break;
        }

        for (int i = 0; i < loadedDialogue.Count; i++)
        {
            dialogue.dialogue.Add(DialogueFilter(loadedDialogue.ElementAt(i).Value, false, state));
            dialogue.title.Add(DialogueFilter(loadedDialogue.ElementAt(i).Key, true, state));
        }

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

    public string DialogueFilter(string data, bool isKey, QuestionState state)
    {
        string output = "";

        switch (isKey)
        {
            case true:
                if (data.Contains("#"))
                {
                    output = data.Split('#')[0];
                }

                switch (state)
                {
                    case QuestionState.Responses:
                        switch (output.Contains("Correct"))
                        {
                            case true:
                                output = output.Split(new string[] { "Correct_" }, System.StringSplitOptions.None)[1];
                                GameManager.playerData.score++;
                                break;
                            case false:
                                output = output.Split(new string[] { "Incorrect_" }, System.StringSplitOptions.None)[1];
                                break;
                        }
                        break;
                }
                break;
            case false:
                List<string> keySeparation = data.Split(new string[] { "|" }, System.StringSplitOptions.None).ToList();

                for (int i = 0; i < keySeparation.Count; i++)
                {
                    if (keySeparation[i].Contains("$T"))
                    {
                        string delay = keySeparation[i].Split(new string[] { "$T" }, System.StringSplitOptions.None)[1];
                        dialogue.delay = float.Parse(delay);
                        keySeparation.RemoveAt(i);
                    }
                    if (keySeparation.Contains("$CE"))
                    { 
                        
                    }
                }

                output = keySeparation[0];
                break;
        }

        return output;
    }
}

public enum QuestionState
{ 
    Dialogue,
    Questions,
    Responses
}