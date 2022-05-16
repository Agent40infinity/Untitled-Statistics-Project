using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DialogueController : MonoBehaviour
{
    [Header("")]
    public GameObject dialogueSelection;
    public GameObject dialogueBox;
    public Dialogue dialogue;
    public List<TextMeshProUGUI> options = new List<TextMeshProUGUI>();

    public string separator = "|";

    public string currentQuestion;

    ProcessedDialogue processedDialogue;

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
            decompiledTitle[i] = DialogueData.currentlyLoaded.levelData[FieldManager.GetState][currentQuestion]["Questions"][(i + 1).ToString()];
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

        Dictionary<string, string> loadedDialogue = DialogueData.currentlyLoaded.levelData[FieldManager.GetState][currentQuestion][state.ToString()];

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

        processedDialogue = new ProcessedDialogue();

        for (int i = 0; i < loadedDialogue.Count; i++)
        {
            processedDialogue.dialogue.Add(DialogueFilter(loadedDialogue.ElementAt(i).Value, false, state));
            processedDialogue.title.Add(DialogueFilter(loadedDialogue.ElementAt(i).Key, true, state));

            ProcessedNullCheck();
        }

        dialogue.dialogue = processedDialogue;
        Debug.Log(DialogueDebug.CheckDialogue(processedDialogue));

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
                                //snGameManager.playerData.score++;
                                break;
                            case false:
                                output = output.Split(new string[] { "Incorrect_" }, System.StringSplitOptions.None)[1];
                                break;
                        }
                        break;
                }
                break;
            case false:
                List<string> keySeparation = data.Split(new string[] { separator }, System.StringSplitOptions.None).ToList();

                foreach (string entry in keySeparation)
                {
                    switch (entry)
                    {
                        case string a when entry.Contains("$T"):
                            string delay = entry.Split(new string[] { "$T" }, System.StringSplitOptions.None)[1];
                            processedDialogue.delay.Add(float.Parse(delay));
                            continue;

                        case string b when entry.Contains("$CE"):
                            string expression = entry.Split(new string[] { "$CE" }, System.StringSplitOptions.None)[1];
                            processedDialogue.expression.Add(expression);
                            continue;

                        case string c when entry.Contains("$BG"):
                            string background = entry.Split(new string[] { "$BG" }, System.StringSplitOptions.None)[1];
                            processedDialogue.background.Add(background);
                            continue;

                        case string d when entry.Contains("$POS"):
                            string position = entry.Split(new string[] { "$POS" }, System.StringSplitOptions.None)[1];
                            bool pos = true;

                            switch (int.Parse(position))
                            {
                                case 1: pos = true; break;
                                case 0: pos = false; break;
                            }

                            processedDialogue.position.Add(pos);
                            continue;
                        case string e when entry.Contains("$VID"):
                            string video = entry.Split(new string[] { "$VID" }, System.StringSplitOptions.None)[1];
                            processedDialogue.feedback.Add(video);
                            Debug.Log("Processed");
                            continue;

                        case string f when entry.Contains("$PT"):
                            string particle = entry.Split(new string[] { "$PT" }, System.StringSplitOptions.None)[1];
                            processedDialogue.particle.Add(particle);
                            continue;

                        case string g when entry.Contains("$SFX"):
                            string sfx = entry.Split(new string[] { "$SFX" }, System.StringSplitOptions.None)[1];
                            processedDialogue.sfx.Add(sfx);
                            continue;

                        case string h when entry.Contains("$BGM"):
                            string bgm = entry.Split(new string[] { "$BGM" }, System.StringSplitOptions.None)[1];
                            processedDialogue.bgm.Add(bgm);
                            continue;
                    }
                }

                output = keySeparation[keySeparation.Count - 1];
                break;
        }

        return output;
    }

    public void ProcessedNullCheck()
    {
        int index = processedDialogue.dialogue.Count;

        if (processedDialogue.delay.Count != index)
        {
            processedDialogue.delay.Add(0);
        }

        if (processedDialogue.background.Count != index)
        {
            processedDialogue.background.Add(null);
        }

        if (processedDialogue.expression.Count != index)
        {
            processedDialogue.expression.Add(null);
        }

        if (processedDialogue.position.Count != index)
        {
            if (processedDialogue.position.Count == 0)
            {
                processedDialogue.position.Add(true);
            }
            else
            {
                processedDialogue.position.Add(processedDialogue.position[processedDialogue.position.Count - 1]);
            }

        }

        if (processedDialogue.feedback.Count != index)
        {
            processedDialogue.feedback.Add(null);
        }

        if (processedDialogue.particle.Count != index)
        {
            processedDialogue.particle.Add(null);
        }

        if (processedDialogue.sfx.Count != index)
        {
            processedDialogue.sfx.Add(null);
        }

        if (processedDialogue.bgm.Count != index)
        {
            processedDialogue.bgm.Add(null);
        }
    }
}

public enum QuestionState
{ 
    Dialogue,
    Questions,
    Responses
}