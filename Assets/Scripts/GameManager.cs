using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Manager")]
    public static GameManager instance;
    public LoadingScreen loadingScreen;
    public static string loadedSaveFile;
    public static Animator saveIcon;
    public FadeController fade;

    [Header("Settings")]
    public static AudioMixer masterMixer; //Creates reference for the menu musi
    public static Dictionary<string, KeyCode> keybind = new Dictionary<string, KeyCode> //Dictionary to store the keybinds.
    {
        { "LookUp", KeyCode.W },
        { "LookDown", KeyCode.S },
        { "MoveLeft", KeyCode.A },
        { "MoveRight", KeyCode.D },
        { "Jump", KeyCode.Space },
        { "Dash", KeyCode.LeftShift },
        { "Attack", KeyCode.E },
        { "Heal", KeyCode.R },
        { "Interact", KeyCode.F },
        { "Pause", KeyCode.Escape }
    };

    public void Awake()
    {
        StartGame();
    }

    public void StartGame()
    {
        instance = this;
        saveIcon = GameObject.FindWithTag("SaveIcon").GetComponent<Animator>();
        masterMixer = Resources.Load("Music/Mixers/Master") as AudioMixer; //Loads the MasterMixer for renference.

        SceneManager.LoadSceneAsync((int)SceneIndex.Test, LoadSceneMode.Additive);
    }

    public void SwapLevel(SceneIndex loadingIndex, SceneIndex previousIndex)
    {
        loadingScreen.Visibility(true);

        List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)previousIndex));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)loadingIndex, LoadSceneMode.Additive));

        StartCoroutine(SceneLoadProgress(scenesLoading));
    }

    public IEnumerator SceneLoadProgress(List<AsyncOperation> scenesLoading)
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }
        }
    }

    public void OnApplicationQuit()
    {
        //Send Notification for incon
    }
}