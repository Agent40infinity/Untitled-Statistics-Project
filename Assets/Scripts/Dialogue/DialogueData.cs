using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DialogueData
{
    public static Level currentlyLoaded = new Level();

    public DialogueData(string input)
    {
        currentlyLoaded = JsonConvert.DeserializeObject<Level>(input);
    }
}