using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public static PlayerData playerData;

    private SheetReader sheetReader;

    [Header("Data Collection")]
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
        sheetReader = new SheetReader(spreadsheetId, jsonPath, sheetRange);
        
        StartCoroutine(playerData.GameTimer());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SaveData();
        }
    }

    public void CallQuestionTimer(string question)
    {
        playerData.timeSpent.Add(question, 0);

        questionState = TrackState.Tracking;
        StartCoroutine(playerData.QuestionTimer(question));
    }

    public IEnumerator LoadDialogueFile()
    {
        string path = Application.streamingAssetsPath + DialogueLoading.fileType["Default"];
        List<string> files = Directory.GetFiles(path).ToList();

        List<string> questionsToAdd = new List<string>();

        foreach (var file in files)
        {
            StreamReader reader = new StreamReader(file);
            string data = reader.ReadToEnd();
            Level level = JsonConvert.DeserializeObject<Level>(data);

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
                            questionsToAdd.Add(level.levelData[field].ElementAt(j).Value.name);
                            break;
                    }
                }
            }
        }

        yield return sheetReader.AppendSheetRange(CompileHeaderData(questionsToAdd));
    }

    public void SaveData()
    {
        gameState = TrackState.Complete;
        StartCoroutine(RetrieveAndSendData());
    }

    public RowList CompileHeaderData()
    {
        RowList rowList = new RowList();
        Row row = new Row();

        row.cellData = new List<string>()
        {
            { "ID" },
            { "Total Time" },
            { "Completed?" },
            { "Feedback" },
        };

        foreach (var question in playerData.questions)
        {
            string node = "Question " + question.Key + ",Required Help on " + question.Key + "?,Time Spent on " + question.Key;
            row.cellData.AddRange(node.Split(',').ToList());
        }

        rowList.rows.Add(row);

        return rowList;
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
            string questions = playerData.questions[question.Key] + "," + playerData.requiredHelp[question.Key] + "," + playerData.timeSpent[question.Key];
            row.cellData.AddRange(questions.Split(',').ToList());
        }

        rowList.rows.Add(row);

        return rowList;
    }

    public IEnumerator RetrieveAndSendData()
    {
        Debug.Log(SheetReader.sheetRange);
        yield return sheetReader.AppendSheetRange(GetPlayerData());
        yield return sheetReader.updateSheetRange(CompileHeaderData(), "1:1");
    }

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