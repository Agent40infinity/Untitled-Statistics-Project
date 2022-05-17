using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
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

    [Header("Feedback Reference")]
    public VideoPlayer videoFeedback;
    public Image imageFeedback;

    [Header("Audio References")]
    public AudioSource sfx;
    public AudioSource bgm;

    public void Update()
    {
        if (dialogueState == DialogueState.Load)
        {
            StartCoroutine(DisplayText(dialogue.dialogue[index])); // Theres a problem here
            DisplayName(dialogue.title[index]);
            DisplayModifiers();
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
                        DisplayModifiers();
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

    public void DisplayModifiers()
    {
        DisplaySprites();
        DisplayEffects();
        ActiveSound();
    }

    public void DisplaySprites()
    {
        if (dialogue.expression[index] != null)
        {
            int position = System.Convert.ToInt32(dialogue.position[index]);
            characters[position].sprite = Resources.Load<Sprite>("Sprites/Characters/" + dialogue.title[index] + "/" + dialogue.title[index] + "_" + dialogue.expression[index]);
        }

        if (dialogue.background[index] != null)
        {
            Debug.Log("Processing");
            background.sprite = Resources.Load<Sprite>("Sprites/Backgrounds/" + dialogue.background[index]);
        }
    }

    public void DisplayEffects()
    {
        if (dialogue.feedback[index] != null)
        {
            string path = "/Feedback/" + dialogue.feedback[index];
            videoFeedback.gameObject.SetActive(true);

            if (dialogue.feedback[index].Contains(".mp4"))
            {
                imageFeedback.enabled = false;
                videoFeedback.enabled = true;
                videoFeedback.clip = Resources.Load<VideoClip>(path);
                videoFeedback.Play();
            }
            else
            {
                imageFeedback.enabled = true;
                videoFeedback.enabled = false;
                imageFeedback.sprite = Resources.Load<Sprite>(path);
            }
        }
        else
        {
            if (videoFeedback.gameObject.activeInHierarchy)
            {
                videoFeedback.gameObject.SetActive(false);
            }
        }
    }

    public void ActiveSound()
    {
        if (dialogue.sfx[index] != null)
        {
            sfx.clip = Resources.Load("Audio/SFX/" + dialogue.sfx[index]) as AudioClip;
            sfx.Play();
            bgm.Stop();

            StartCoroutine(CheckSFX());
        }

        if (dialogue.bgm[index] != null)
        {
            bgm.clip = Resources.Load("Audio/Background/" + dialogue.bgm[index]) as AudioClip;
            bgm.Play();
        }
        else if (dialogue.bgm[index] == "none")
        {
            bgm.Stop();
            bgm.clip = null;
        }
    }

    public IEnumerator CheckSFX()
    {
        if (sfx.isPlaying)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        bgm.Play();
    }
}

public enum DialogueState
{
    Load,
    Idle,
    Normal,
    Fast
}