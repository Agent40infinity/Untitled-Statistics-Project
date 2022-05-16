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


    public void Awake()
    {
        StartCoroutine(SendData());
    }
    public string GetPlayerData()
    {
        string id = "";

        string questions = "";
        for (int i = 0; i < playerData.questions.Count; i++)
        {
            questions += playerData.questions[i] 
                + "," + playerData.requiredHelp[i]
                + "," + playerData.timeSpent[i];
        }

        return id + "," + playerData.completion + "," + questions + "," + playerData.feedback;
    }

    public IEnumerator SendData()
    {
        if (Application.isEditor)
        {
            url = Application.streamingAssetsPath + "/PlayerData/" + "Database.csv";
        }

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            yield return null;
        }

        List<string> csv = www.downloadHandler.text.Split(new string[] { "," }, System.StringSplitOptions.None).ToList();
        //TextWriter tw = new StreamWriter(www.downloadHandler.text);
        for (int i = 0; i < csv.Count; i++)
        {
            Debug.Log(csv[i]);
        }
        //tw.WriteLine(GetPlayerData());
    }
}
