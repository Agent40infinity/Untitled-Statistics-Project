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

    public int sectionIndex;
    public static bool queueWaiting = true;

    public string separator = "|";

    public Question currentQuestion;

    public bool isNext = true;

    public bool wasCorrect = false;

    ProcessedDialogue processedDialogue;

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
        switch (sectionIndex)
        {
            case int a when sectionIndex > 0 && sectionIndex < currentQuestion.sections.Count:
                if (currentQuestion.sections[sectionIndex - 1].state == QuestionState.Questions || (currentQuestion.sections[sectionIndex].state == QuestionState.Questions && !wasCorrect))
                {
                    sectionIndex += 2;
                }
                else
                {
                    sectionIndex++;
                }
                break;
            default:
                sectionIndex++;
                break;
        }

        if (sectionIndex < currentQuestion.sections.Count)
        {
            LoadSection();
            queueWaiting = true;
            return;
        }

        LevelManager.queueWaiting = false;
    }

    public void QuestionSetup(Question question)
    {
        currentQuestion = question;
        sectionIndex = -1;
        queueWaiting = false;
    }

    public void LoadSection()
    {
        switch (currentQuestion.sections[sectionIndex].state)
        {
            case QuestionState.Questions: case QuestionState.Alternative:
                StartCoroutine(DialogueSelection());
                break;
            case QuestionState.Dialogue: case QuestionState.ResponseCorrect: case QuestionState.ResponseIncorrect:
                DisplayDialogue();
                break;
        }
    }

    public IEnumerator DialogueSelection()
    {
        dialogueSelection.SetActive(true);
        DataManager.instance.CallQuestionTimer(currentQuestion.name);

        int dialogueOptions = currentQuestion.sections[sectionIndex].values.Count;

        for (int i = 0; i < options.Count; i++)
        {
            if (dialogueOptions > 0)
            {
                options[i].transform.parent.gameObject.SetActive(true);
                options[i].text = currentQuestion.sections[sectionIndex].values.ElementAt(i).Value;
                dialogueOptions--;
            }
            else 
            {
                options[i].transform.parent.gameObject.SetActive(false);
            }
        }

        yield return null;
    }

    public void OptionSelection(int optionIndex)
    {
        StartCoroutine(ResponseCheck(optionIndex));
    }

    public IEnumerator ResponseCheck(int optionIndex)
    {
        switch (currentQuestion.sections[sectionIndex].state)
        {
            case QuestionState.Alternative:
                isNext = System.Convert.ToBoolean(optionIndex);
                break;
            default:
                switch (int.Parse(currentQuestion.answer))
                {
                    case int a when a == optionIndex:
                        wasCorrect = true;
                        break;
                    default:
                        wasCorrect = false;
                        break;
                }
                break;
        }

        queueWaiting = false;
        yield return null;
    }

    public void DialogueActivation(bool toggle)
    {
        dialogueSelection.SetActive(!toggle);
        dialogueBox.SetActive(toggle);
    }

    public void DisplayDialogue()
    {
        DialogueActivation(true);

        switch (currentQuestion.sections[sectionIndex].state)
        {
            case QuestionState.ResponseCorrect: case QuestionState.ResponseIncorrect:

                DataManager.playerData.questions.Add(currentQuestion.name, wasCorrect);
                DataManager.instance.questionState = TrackState.Complete;
                break;
        }

        processedDialogue = new ProcessedDialogue();

        for (int i = 0; i < currentQuestion.sections[sectionIndex].values.Count; i++)
        {
            processedDialogue.title.Add(DialogueFilter(currentQuestion.sections[sectionIndex].values.ElementAt(i).Key, true));
            processedDialogue.dialogue.Add(DialogueFilter(currentQuestion.sections[sectionIndex].values.ElementAt(i).Value, false));

            ProcessedNullCheck();
        }

        dialogue.dialogue = processedDialogue;
        Debug.Log(DialogueDebug.CheckDialogue(processedDialogue));

        dialogue.dialogueState = DialogueState.Load;
    }

    public string DialogueFilter(string data, bool isKey)
    {
        string output = "";

        switch (isKey)
        {
            case true:
                if (data.Contains("#"))
                {
                    output = data.Split('#')[0];
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
                            string vid = entry.Split(new string[] { "$VID" }, System.StringSplitOptions.None)[1];
                            processedDialogue.feedback.Add(vid);
                            continue;

                        case string i when entry.Contains("$PIC"):
                            string pic = entry.Split(new string[] { "$PIC" }, System.StringSplitOptions.None)[1];
                            processedDialogue.feedback.Add(pic);
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
    ResponseCorrect,
    ResponseIncorrect,
    Alternative,
}