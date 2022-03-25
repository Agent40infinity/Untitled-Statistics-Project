using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Dialogue : MonoBehaviour
{
    public float[] interval = new float[2];
    public int intervalIndex;
    public List<string> dialogue = new List<string>();
    public List<string> title = new List<string>();
    public int index;
    public string display;
    public DialogueState dialogueState = DialogueState.Idle;

    public TextMeshProUGUI characterText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueParent;

    public void Update()
    {
        if (dialogueState == DialogueState.Load)
        {
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
            StartCoroutine(DisplayText(dialogue[index])); // Theres a problem here
            DisplayName(title[index]);
=======
>>>>>>> Stashed changes
            StartCoroutine(DisplayText(dialogue.ElementAt(index).Value));
            DisplayName(DialogueController.DialogueFilter(dialogue.ElementAt(index).Key, true));
>>>>>>> 739ac809ccde320ebc8be76ae7191eb4d8ab34fd
            dialogueState = DialogueState.Normal;
        }

        if (Input.GetMouseButtonDown(0) && dialogueParent.activeInHierarchy)
        {
            if (index < dialogue.Count)
            {
                switch (dialogueState)
                {
                    case DialogueState.Idle:
                        StartCoroutine(DisplayText(dialogue[index]));
                        DisplayName(title[index]);
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
                ClearDialogue();
            }
        }
    }

    public void ClearDialogue()
    {
        dialogue.Clear();
        intervalIndex = 0;
        index = 0;
        dialogueParent.SetActive(false);
        LevelManager.queueWaiting = false;
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
}

public enum DialogueState
{
    Load,
    Idle,
    Normal,
    Fast
}