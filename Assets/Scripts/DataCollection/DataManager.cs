using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
<<<<<<< Updated upstream
=======
using System.IO;
using UnityEngine.Networking;

using Newtonsoft.Json;
>>>>>>> Stashed changes

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public static PlayerData playerData;
    private List<string> csv;

    [Header("Data Collection")]
    public string serviceEmail = "statistics-project@statistics-project-345715.iam.gserviceaccount.com";
    public string spreadsheetId = "1_IZo5MzTXkgZSyTK8tM9iIsWnkhxjMQYCci60fCArdY";
    public string jsonPath = "/PlayerData/statistics-project-345715-8b6c8278d652.json";
    public string sheetRange = "Player Data";

    [Header("Tracking")]
    public TrackState gameState = TrackState.Tracking;
    public TrackState questionState = TrackState.Complete;


    public void Awake()
    {
        instance = this;
        playerData = new PlayerData();
<<<<<<< Updated upstream
        GoogleAuth();
=======
        sheetReader = new SheetReader(serviceEmail ,spreadsheetId, jsonPath, sheetRange);
        
>>>>>>> Stashed changes
        StartCoroutine(playerData.GameTimer());
        StartCoroutine(LoadDialogueFile());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SaveData();
        }
    }

    public void GoogleAuth()
    {
        SheetReader.spreadsheetId = spreadsheetId;
        SheetReader.jsonPath = jsonPath;
        SheetReader.sheetRange = sheetRange;
    }

    public void CallQuestionTimer(string question)
    {
        playerData.timeSpent[question] = 0;

        questionState = TrackState.Tracking;
        StartCoroutine(playerData.QuestionTimer(question));
    }

<<<<<<< Updated upstream
    public void SaveData()
=======
    public IEnumerator LoadDialogueFile()
    {
        string path = Application.streamingAssetsPath + DialogueLoading.fileType["Default"];
        List<string> files = Directory.GetFiles(path).ToList();
        files.RemoveAll(ContainsMeta);

        List<string> questionsToAdd = new List<string>();

        foreach (var file in files)
        {
            string data = "";

            if (Application.isEditor)
            {
                StreamReader reader = new StreamReader(file);
                data = reader.ReadToEnd();
            }
            else
            {
                UnityWebRequest www = UnityWebRequest.Get(path);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }

                data = www.downloadHandler.text;
            }

            Level level = JsonConvert.DeserializeObject<Level>(data);
            questionsToAdd.AddRange(RequestQuestions(level));
        }

        while (!sheetReader.serviceLoaded)
        {
            yield return null;
        }

        yield return sheetReader.updateSheetRange(CompileHeaderData(questionsToAdd), "1:1");
        yield return PlayerDataAdjust(questionsToAdd);
    }

    public bool ContainsMeta(string s)
>>>>>>> Stashed changes
    {
        return s.Contains(".meta");
    }

    public List<string> RequestQuestions(Level level)
    {
        List<string> output = new List<string>();

        for (int i = 0; i < FieldManager.FieldCount; i++)
        {
            FieldState field = FieldManager.GetIndexOf(i);

            for (int j = 0; j < level.levelData[field].Count; j++)
            {
                switch (level.levelData[field].ElementAt(j).Value.answer)
                {
                    case "":
                        break;
                    default:
                        output.Add(level.levelData[field].ElementAt(j).Value.name);
                        break;
                }
            }
        }

        return output;
    }

    public RowList CompileHeaderData(List<string> questions)
    {
        RowList rowList = new RowList();
        Row row = new Row();

        row.cellData = new List<string>("ID,Total Time,Completed?,Feedback".Split(',').ToList());

        foreach (var question in questions)
        {
<<<<<<< Updated upstream
            string node = ",Question " + question.Key + ",Required Help on " + question.Key + "?,Time Spent on " + question.Key;
            row.cellData.Concat(node.Split(',').ToList());
=======
            string node = "Question " + question + ",Time Spent on " + question + ",Required Help on " + question + "?";
            row.cellData.AddRange(node.Split(',').ToList());
>>>>>>> Stashed changes
        }

        rowList.rows.Add(row);

        return rowList;
    }

    public IEnumerator PlayerDataAdjust(List<string> questions)
    {
        foreach (var question in questions)
        {
            playerData.questions.Add(question, null);
            playerData.requiredHelp.Add(question, null);
            playerData.timeSpent.Add(question, null);
        }

        yield return null;
    }

    public void SaveData()
    {
        gameState = TrackState.Complete;
        StartCoroutine(RetrieveAndSendData());
    }

    public IEnumerator RetrieveAndSendData()
    {
        Debug.Log(SheetReader.sheetRange);
        yield return sheetReader.AppendSheetRange(GetPlayerData());
    }

    public RowList GetPlayerData()
    {
        RowList rowList = new RowList();
        Row row = new Row();

        row.cellData = new List<string>()
        {
            { System.Guid.NewGuid().ToString() },
            { playerData.totalTime.ToString() },
            { playerData.completion.ToString() },
            { playerData.feedback },
        };
        
        foreach (var question in playerData.questions)
        {
<<<<<<< Updated upstream
            string questions = playerData.questions[question.Key] + "," + playerData.requiredHelp[question.Key] + "," + playerData.timeSpent[question.Key];
            row.cellData.Concat(questions.Split(',').ToList());
=======
            string questions = playerData.questions[question.Key] + "," + playerData.timeSpent[question.Key] + "," + playerData.requiredHelp[question.Key];
            row.cellData.AddRange(questions.Split(',').ToList());
>>>>>>> Stashed changes
        }

        rowList.rows.Add(row);

        return rowList;
    }

<<<<<<< Updated upstream
    public IEnumerator RetrieveAndSendData()
    {
        SheetReader sheetReader = new SheetReader();
        yield return sheetReader.updateSheetRange(CompileHeaderData(), "1:1");

        yield return sheetReader.AppendSheetRange(GetPlayerData());
    }

=======
>>>>>>> Stashed changes
    public void OnApplicationQuit()
    {
        SaveData();
    }
}

public enum TrackState
{ 
    Tracking,
    Complete
}