using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class DialogueLoading : MonoBehaviour
{
    public static DialogueLoading instance;

    public static Dictionary<string, string> fileType = new Dictionary<string, string>()
    {
        {"Default", "/Dialogue/"},
    };

    public DataLoaded dataLoaded;

    public void Awake()
    {
        instance = this;
    }

    public IEnumerator LoadDialogue(string name)    
    {
        dataLoaded = DataLoaded.Unloaded;

        string path = Application.streamingAssetsPath + fileType["Default"] + name + ".json";

        if (Application.isEditor)
        {
            StreamReader reader = new StreamReader(path);
            string data = reader.ReadToEnd();
            DialogueData dialogueData = new DialogueData(data);
            dataLoaded = DataLoaded.Loaded;
        }
        else
        {
            yield return RequestDialogue(path);
        }
    }

    public IEnumerator RequestDialogue(string path)
    {
        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }

        DialogueData dialogueData = new DialogueData(www.downloadHandler.text);
        dataLoaded = DataLoaded.Loaded;
        Debug.Log("loaded");
    }

    public bool HasLoaded()
    {
        switch (dataLoaded)
        {
            case DataLoaded.Loaded:
                return true;
            default:
                return false;
        }
    }

    public void GenerateLevel()
    {
        Level level = new Level();

        string path = Application.streamingAssetsPath + fileType["Default"] + "Level" + Random.Range(0, 999) + ".json"; //Gets the file in directory.
        string json = JsonConvert.SerializeObject(level, Formatting.Indented); //Creates a new SettingData so that the data can be serialised.
        StreamWriter writer = File.CreateText(path); //Overrides/Creates a new file for settings based on the path and data provided.
        writer.Close();

        File.WriteAllText(path, json); //Saves the data to the .json using the json sring and path information.
    }
}

public enum DataLoaded
{ 
    Unloaded,
    Loaded
}