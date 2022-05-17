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

    public IEnumerator RetrieveData()
    {
        if (Application.isEditor)
        {
            url = Application.streamingAssetsPath + "/PlayerData/" + "weather.data.csv";
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
        List<string> csv = unprocessed.Split(new string[] { "," }, System.StringSplitOptions.None).ToList();
    }
}
