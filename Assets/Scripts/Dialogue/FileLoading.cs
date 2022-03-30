using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using Newtonsoft.Json;

public class DialogueLoading : MonoBehaviour
{
    public static Dictionary<string, string> fileType = new Dictionary<string, string>()
    {
        {"Default", "/Data/Dialogue/"},
        {"Player", "/Data/Dialogue/"},
    };

    public static void LoadDialogue(string name)
    {
        StreamReader reader = new StreamReader(Application.dataPath + fileType["Default"] + name + ".json");
        string data = reader.ReadToEnd();
        DialogueData dialogueData = new DialogueData(data);
        Debug.Log("loaded");
    }

    public static void SavePlayerData()
    { 
        //if (Application.dataPath + fileType["Default"])
    }

    public static void SaveLevel(Level level)
    {
        string path = Application.dataPath + fileType["Default"] + "testLevel.json"; //Gets the file in directory.
        string json = JsonConvert.SerializeObject(level, Formatting.Indented); //Creates a new SettingData so that the data can be serialised.
        StreamWriter writer = File.CreateText(path); //Overrides/Creates a new file for settings based on the path and data provided.
        writer.Close();

        File.WriteAllText(path, json); //Saves the data to the .json using the json sring and path information.
    }
}