using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    [Header("Attributes")]
    public string level;
    public Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>> levelData = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>();
    public Dictionary<string, int> answers = new Dictionary<string, int>();
}
