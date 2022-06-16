using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;


using UnityEngine;

using Newtonsoft.Json;

public class SheetReader
{
    static public String spreadsheetId = "1_IZo5MzTXkgZSyTK8tM9iIsWnkhxjMQYCci60fCArdY";
    static public String jsonPath = "/PlayerData/statistics-project-345715-8b6c8278d652.json";
    static public String sheetRange = "Player Data";

    static private SheetsService service;

    public SheetReader(String id, String path, String range)
    {
        GoogleAuth(id, path, range);

        String fullJsonPath = Application.streamingAssetsPath + jsonPath;

        Stream jsonCreds = (Stream)File.Open(fullJsonPath, FileMode.Open);

        ServiceAccountCredential credential = ServiceAccountCredential.FromServiceAccountData(jsonCreds);

        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
        });
    }

    public void GoogleAuth(String id, String path, String range)
    {
        spreadsheetId = id;
        jsonPath = path;
        sheetRange = range;
    }

    public IList<IList<object>> getSheetRange(String cells)
    {
        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, sheetRange + cells);

        ValueRange response = request.Execute();
        IList<IList<object>> values = response.Values;

        if (values != null && values.Count > 0)
        {
            return values;
        }
        else
        {
            Debug.Log("No data found.");
            return null;
        }
    }

    public IEnumerator updateSheetRange(RowList dataToWrite, string range)
    {
        ValueRange valueRange = RegisterValueRange(dataToWrite);

        SpreadsheetsResource.ValuesResource.UpdateRequest request = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, sheetRange + "!" + range);
        request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
        var response = request.Execute();
        Debug.Log(response);

        yield return null;
    }

    public IEnumerator AppendSheetRange(RowList dataToWrite)
    {
        ValueRange valueRange = RegisterValueRange(dataToWrite);

        SpreadsheetsResource.ValuesResource.AppendRequest request = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, sheetRange);
        request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        var response = request.Execute();
        Debug.Log(response);

        yield return null;
    }

    public ValueRange RegisterValueRange(RowList dataToWrite)
    {
        ValueRange valueRange = new ValueRange();
        var rows = new List<IList<object>>();

        foreach (var row in dataToWrite.rows)
        {
            var cellData = new List<object>();

            foreach (var cell in row.cellData)
            {
                cellData.Add(cell);
            }

            rows.Add(cellData);
        }

        valueRange.Values = rows;
        return valueRange;
    }

    public void SaveSheet(IList<IList<object>> sheetRange)
    {
        string path = Application.streamingAssetsPath + "DebugOutput.json"; //Gets the file in directory.
        string json = JsonConvert.SerializeObject(sheetRange, Formatting.Indented); //Creates a new SettingData so that the data can be serialised.
        StreamWriter writer = File.CreateText(path); //Overrides/Creates a new file for settings based on the path and data provided.
        writer.Close();

        File.WriteAllText(path, json); //Saves the data to the .json using the json sring and path information.
    }
}

public class Row
{
    public List<string> cellData = new List<string>();
}

[Serializable]
public class RowList
{
    public List<Row> rows = new List<Row>();
}