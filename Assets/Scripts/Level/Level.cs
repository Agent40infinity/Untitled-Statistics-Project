using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    [Header("Attributes")]
    public string level = "1";
    public Dictionary<FieldState, Dictionary<string, Question>> levelData = new Dictionary<FieldState, Dictionary<string, Question>>();
}

public class Question
{
    public string name = "";
    public string next = "";
    public string alternative = "";
    public AnswerType answerType = AnswerType.Multi;
    public string answer = "1";
    public List<Section> sections;
}

public class Section
{
    public string name = "";
    public QuestionState state = QuestionState.Dialogue;
    public Dictionary<string, string> values = new Dictionary<string, string>();
}

public enum AnswerType
{
    Short,
    Multi,
}