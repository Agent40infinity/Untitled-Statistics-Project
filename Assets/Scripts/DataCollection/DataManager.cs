using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static PlayerData playerData;

    [Header("Data Collection")]
    public string url;
    private int questionCount = 4;


    public void Awake()
    {
        playerData = new PlayerData();
    }

    public void SaveData()
    {
        StartCoroutine(RetrieveData());
    }

    public string CompileDataHeader()
    {
        string header = "ID,Total Time,Completed?,Feedback";

        for (int i = 0; i < playerData.questions.Count; i++)
        {
            header += ",Question " + (i + 1) + "Required Help on " + (i + 1) + "?,Time Spent on " + (i + 1);
            questionCount++;
        }

        return header;
    }

    public string GetPlayerData()
    {
        string id = System.Guid.NewGuid().ToString();

        string questions = "";
        for (int i = 0; i < playerData.questions.Count; i++)
        {
            questions += playerData.questions[i] 
                + "," + playerData.requiredHelp[i]
                + "," + playerData.timeSpent[i];
        }

        return id + "," + playerData.completion + "," + questions + "," + playerData.feedback;
    }

    public IEnumerator RetrieveData()
    {
        if (Application.isEditor)
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
        List<string> csv = unprocessed.Split(new string[] { "\n" }, System.StringSplitOptions.None).ToList();

        yield return CompileData(csv);
    }

    public IEnumerator CompileData(List<string> csv)
    {
        if (playerData.completion)
        {
            string header = CompileDataHeader();
            csv[0] = header;
        }

        csv.Add(GetPlayerData());
        yield return SendData(csv);


        /*List<string> lastLine = csv[csv.Count - 1].Split(new string[] { "," }, System.StringSplitOptions.None).ToList();
        int newID = int.Parse(lastLine[0]) + 1;

        List<List<string>> compiledData = new List<List<string>>();

        for (int i = 0; i < csv.Count / questionCount; i++)
        {
            List<string> lineData = new List<string>();

            for (int j = 0; j < questionCount; j++)
            {
                lineData.Add(csv[i * questionCount + j]);
            }

            compiledData.Add(lineData);
        }*/
    }

    public IEnumerator SendData(List<string> csv)
    {
        yield return null;
    }
}
