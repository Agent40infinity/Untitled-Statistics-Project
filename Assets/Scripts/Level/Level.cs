using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    [Header("Attributes")]
    public string level;
    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> levelData = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
    /*{
        {"Q115", new Dictionary<string, Dictionary<string, string>>
        {
            { "Dialogue", new Dictionary<string, string>
            {
                {"Zoe", "Hi my name is Zoe"},
                {"Hannah", "Same"},
            } },
            { "Questions", new Dictionary<string, string>
            {
                {"Zoe", "Hi my name is Zoe"},
                {"Hannah", "Same"},
            } },
            { "Responses", new Dictionary<string, string>
            {
                {"Zoe", "Hi my name is Zoe"},
                {"Hannah", "Same"},
            } },
        } }
    };*/
    public Dictionary<string, string> answers = new Dictionary<string, string>();
}
