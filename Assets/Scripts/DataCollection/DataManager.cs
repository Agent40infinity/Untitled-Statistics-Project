using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public static PlayerData playerData;
    private List<string> csv;

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
        GoogleAuth();
        StartCoroutine(playerData.GameTimer());
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
        playerData.timeSpent.Add(question, 0);

        questionState = TrackState.Tracking;
        StartCoroutine(playerData.QuestionTimer(question));
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

        row.cellData = new List<string>("ID,Total Time,Completed?,Feedback".Split(',').ToList());

        foreach (var question in playerData.questions)
        {
            string node = ",Question " + question.Key + ",Required Help on " + question.Key + "?,Time Spent on " + question.Key;
            row.cellData.Concat(node.Split(',').ToList());
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
            row.cellData.Concat(questions.Split(',').ToList());
        }

        rowList.rows.Add(row);

        return rowList;
    }

    public IEnumerator RetrieveAndSendData()
    {
        SheetReader sheetReader = new SheetReader();
        yield return sheetReader.updateSheetRange(CompileHeaderData(), "1:1");

        yield return sheetReader.AppendSheetRange(GetPlayerData());
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