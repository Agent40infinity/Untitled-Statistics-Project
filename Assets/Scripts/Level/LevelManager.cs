using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string identity;

    public DialogueController dialogueController;

    public void Awake()
    {
        DialogueLoading.SaveLevel(new Level());
    }

    public void LoadLevel(string identity)
    {
        dialogueController.Setup(identity);
    }
}
