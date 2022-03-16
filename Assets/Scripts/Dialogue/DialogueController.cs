using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public GameObject dialogueSelection;
    public GameObject dialogueBox;
    public Dialogue dialogue;
    public GameObject secretOption;
    public List<TextMeshProUGUI> options = new List<TextMeshProUGUI>();
    public string[] modifiers =
    {
        "$t#",
        "$s#"
    };

    public void Setup(string level)
    {
        DialogueLoading.LoadDialogue(level);
        InspectSetup();
    }

    public void InspectSetup()
    {
        dialogueSelection.SetActive(true);

        string[] decompiledTitle = new string[options.Count];

        /*for (int i = 0; i < decompiledTitle.Length; i++)
        {
            Debug.Log(DialogueData.currentlyLoaded["Option" + (i + 1)]);
            decompiledTitle[i] = DialogueData.currentlyLoaded["Option" + (i + 1)];
            int index = decompiledTitle[i].IndexOf(modifiers[0]);
            decompiledTitle[i] = decompiledTitle[i].Substring(0, index);
        }*/

        for (int i = 0; i < options.Count; i++)
        {
            options[i].text = decompiledTitle[i];
        }
    }

    public void StorySetup() 
    {
        //Don't touch, think up an easy way to queue dialogue for in order to end the storyline (Most likely will have a seperate script called upon in here called Story that contains excess data).
    }

    public void OptionSelection(int optionIndex)
    {
        InspectLoad(optionIndex);
    }

    public void InspectLoad(int optionIndex)
    {
        DialogueActivation();

        string loadedDialogue = "";

        //loadedDialogue = DialogueData.currentlyLoaded["Option" + optionIndex.ToString()];
        int index = loadedDialogue.IndexOf(modifiers[0]) + modifiers[0].Length;
        loadedDialogue = loadedDialogue.Substring(index, loadedDialogue.Length - index);

        if (loadedDialogue.Contains(modifiers[1]))
        {
            string[] processedDialogue = loadedDialogue.Split(new string[] { modifiers[1] }, System.StringSplitOptions.None);

            for (int i = 0; i < processedDialogue.Length; i++)
            {
                dialogue.dialogue.Add(processedDialogue[i]);
            }
        }
        else
        {
            dialogue.dialogue.Add(loadedDialogue);
        }

        dialogue.dialogueState = DialogueState.Load;
    }

    public void DialogueActivation()
    {
        dialogueSelection.SetActive(false);
        dialogueBox.SetActive(true);
    }
}
