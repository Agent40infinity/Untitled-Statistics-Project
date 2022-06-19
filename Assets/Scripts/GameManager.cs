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
    public int levelIndex;
    public SceneIndex startingScene;
    public List<SceneIndex> exclusionScenes;
    public DialogueController dialogueController;
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
        dialogueController = GameObject.FindWithTag("Dialogue").GetComponent<DialogueController>();

        SceneManager.LoadSceneAsync((int)startingScene, LoadSceneMode.Additive);
        levelIndex = (int)startingScene;

        Exclusion();
    }

    public void SwapLevel()
    {
        loadingScreen.Visibility(true);

        List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            int buildIndex = SceneManager.GetSceneAt(i).buildIndex;

            switch (buildIndex)
            {
                case (int)SceneIndex.Persistent:
                    break;
                default:
                    scenesLoading.Add(SceneManager.UnloadSceneAsync(buildIndex));
                    break;
            }
        }

        levelIndex++;
        if (!FieldManager.CheckComplete && levelIndex == System.Enum.GetValues(typeof(SceneIndex)).Length - 1)
        {
            FieldManager.Completed = FieldManager.State;
            scenesLoading.Add(SceneManager.LoadSceneAsync(FieldManager.selector, LoadSceneMode.Additive));
            levelIndex = 2;
        }
        else if (levelIndex < System.Enum.GetValues(typeof(SceneIndex)).Length)
        {
            scenesLoading.Add(SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive));
        }

        Exclusion();

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

        if (!DialogueLoading.instance.HasLoaded())
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        Debug.Log("?");
        yield return fade.FadeOut(name);
        loadingScreen.Visibility(false);
    }

    public void Exclusion()
    {
        bool exclude = true;

        for (int j = 0; j < exclusionScenes.Count; j++)
        {
            if (levelIndex == (int)exclusionScenes[j])
            {
                exclude = false;
                break;
            }
        }

        ToggleDialogue(exclude);
    }

    public void ToggleDialogue(bool toggle)
    {
        dialogueController.gameObject.SetActive(toggle);
    }
}