using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Dialogue : MonoBehaviour
{
    [Header("Dialogue")]
    public float[] interval = new float[2];
    public int intervalIndex;
    public ProcessedDialogue dialogue; 
    public bool dialoguePaused;
    public int index;
    public string display;
    public DialogueState dialogueState = DialogueState.Idle;

    [Header("Dialogue References")]
    public TextMeshProUGUI characterText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueParent;

    [Header("Sprite References")]
    public Image background;
    public Image[] characters = new Image[2];
    private string[] lastCharacter = new string[2];

    public void Update()
    {
        if (dialogueState == DialogueState.Load)
        {
            StartCoroutine(DisplayText(dialogue.dialogue[index])); // Theres a problem here
            DisplayName(dialogue.title[index]);
            dialogueState = DialogueState.Normal;
        }

        if (Input.GetMouseButtonDown(0) && dialogueParent.activeInHierarchy && !dialoguePaused)
        {
            if (index < dialogue.dialogue.Count)
            {
                switch (dialogueState)
                {
                    case DialogueState.Idle:
                        StartCoroutine(DisplayText(dialogue.dialogue[index]));
                        DisplayName(dialogue.title[index]);
                        DisplaySprites();
                        dialogueState = DialogueState.Normal;
                        break;
                    case DialogueState.Normal:
                        intervalIndex = 1;
                        dialogueState = DialogueState.Fast;
                        break;
                }
            }
            else
            {
                StartCoroutine(ClearDialogue());
            }
        }
    }

    public IEnumerator ClearDialogue()
    {
        yield return StartCoroutine(DialogueDelay());

        dialogue = null;
        intervalIndex = 0;
        index = 0;
        dialogueParent.SetActive(false);
        LevelManager.queueWaiting = false;
    }

    public IEnumerator DialogueDelay()
    {
        dialoguePaused = true;
        yield return new WaitForSeconds(dialogue.delay[index - 1]);
        dialoguePaused = false;
    }

    public void UpdateText()
    {
        dialogueText.text = display;
    }

    public void DisplayName(string name)
    {
        characterText.text = name;
    }

    public IEnumerator DisplayText(string sentence)
    {
        for (int i = 0; i <= sentence.Length; i++)
        {
            display = sentence.Substring(0, i);
            UpdateText();
            yield return new WaitForSeconds(interval[intervalIndex]);
            
        }

        index++;
        intervalIndex = 0;
        dialogueState = DialogueState.Idle;
    }

    public void DisplaySprites()
    {
        Debug.Log(dialogue.position[index].ToString());
        Debug.Log(System.Convert.ToInt32(dialogue.position[index]));
        if (dialogue.expression[index] != null)
        {
            int position = 0;   
            /*if (dialogue.position[index] == null)
            {
                for (int i = 0; i < lastCharacter.Length; i++)
                {
                    if (lastCharacter[i] == dialogue.title[index])
                    {
                        position = i;
                    }
                }
            }
            else
            { */
                position = System.Convert.ToInt32(dialogue.position[index]);
                lastCharacter[position] = dialogue.title[index];
            //}
            characters[position].sprite = Resources.Load<Sprite>("Sprites/Characters/" + dialogue.title[index] + "/" + dialogue.title[index] + "_" + dialogue.expression[index]);
        }

        if (dialogue.background[index] != null)
        {
            background.sprite = Resources.Load("Sprites/Backgrounds/" + dialogue.background[index]) as Sprite;
        }
    }
}

public enum DialogueState
{
    Load,
    Idle,
    Normal,
    Fast
}