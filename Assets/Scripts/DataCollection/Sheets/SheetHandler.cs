using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

public class SheetHandler : MonoBehaviour
{
    [Header("Sheet to Create")]
    public string sheetName = "It worked";
    public string folderPath = "/Dialogue/";

    [Header("Google Auth")]
    public string spreadsheetId = "1_IZo5MzTXkgZSyTK8tM9iIsWnkhxjMQYCci60fCArdY";
    public string jsonPath = "/PlayerData/statistics-project-345715-8b6c8278d652.json";

    public SheetReader sheetReader;
    
    public void Awake()
    {
        //sheetReader = new SheetReader(email, spreadsheetId, jsonPath, sheetName);

        StartCoroutine(LoadDialogueFile());
    }

    public IEnumerator LoadDialogueFile()
    {
        string path = Application.streamingAssetsPath + folderPath;
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

    public RowList CompileHeaderData(List<string> questions)
    {
        RowList rowList = new RowList();
        Row row = new Row();

        row.cellData = new List<string>("ID,Total Time,Completed?,Feedback".Split(',').ToList());

        foreach (var question in questions)
        {
            string node = ",Question " + question + ",Required Help on " + question + "?,Time Spent on " + question;
            List<string> nodes = node.Split(',').ToList();

            foreach (var item in nodes)
            {
                row.cellData.Add(item);
            }
        }

        rowList.rows.Add(row);
        return rowList;
    }
}
