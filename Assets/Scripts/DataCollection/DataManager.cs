using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public static PlayerData playerData;
    private List<string> csv;
 
    [Header("Data Collection")]
    public string url;

    [Header("Tracking")]
    public TrackState gameState = TrackState.Tracking;
    public TrackState questionState = TrackState.Complete;


    public void Awake()
    {
        instance = this;
        playerData = new PlayerData();
        StartCoroutine(playerData.GameTimer());
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
        StartCoroutine(RetrieveData());
    }

    public string CompileDataHeader()
    {
        string header = "ID,Total Time,Completed?,Feedback";

        foreach (var question in playerData.questions)
        {
            header += ",Question " + question.Key + ",Required Help on " + question.Key + "?,Time Spent on " + question.Key;
        }

        return header;
    }

    public string GetPlayerData()   
    {
        string id = System.Guid.NewGuid().ToString();

        string questions = "";
        foreach (var question in playerData.questions)
        {
            questions += "," + playerData.questions[question.Key]
                + "," + playerData.requiredHelp[question.Key]
                + "," + playerData.timeSpent[question.Key];
        }

        return id + "," + playerData.totalTime + "," + playerData.completion + "," + playerData.feedback + questions;
    }

    public IEnumerator RetrieveData()
    {
        if (Application.isEditor || url == "" || url == null)
        {
            url = Application.streamingAssetsPath + "/PlayerData/" + "Data.csv";
        }

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            yield return null;
        }

        byte[] data = www.downloadHandler.data;
        string unprocessed = System.Text.Encoding.Default.GetString(data);
        Debug.Log(unprocessed);
        csv = unprocessed.Split(new string[] { "\n" }, System.StringSplitOptions.None).ToList();

        yield return CompileData();
    }

    public IEnumerator CompileData()
    {

        string header = CompileDataHeader();
        csv.Add(GetPlayerData());

        string compiled = header;

        for (int i = 1; i < csv.Count; i++)
        {
            compiled += "\n" + csv[i];
        }

        Debug.Log(compiled);

        yield return SendData(compiled);
    }

    public IEnumerator SendData(string compiledData)
    {
        if (Application.isEditor)
        {
            StreamWriter writer = File.CreateText(url);
            writer.Close();

            File.WriteAllText(url, compiledData);
        }

        UnityWebRequest www = UnityWebRequest.Post(url, compiledData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            yield return null;
        }
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