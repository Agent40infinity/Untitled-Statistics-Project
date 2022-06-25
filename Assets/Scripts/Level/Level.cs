using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    [Header("Attributes")]
    public string level = "";
    public Dictionary<FieldState, Dictionary<string, Question>> levelData = new Dictionary<FieldState, Dictionary<string, Question>>()
    {
        { FieldState.Poverty, new Dictionary<string, Question>() { { "QuestionName", new Question() }  } },
        { FieldState.Health, new Dictionary<string, Question>() { { "QuestionName", new Question() }  } },
        { FieldState.Education, new Dictionary<string, Question>() { { "QuestionName", new Question() }  } }
    };
}

public class Question
{
    public string name = "";
    public string next = "";
    public string alternative = "";
    public AnswerType answerType = AnswerType.Multi;
    public string answer = "";
    public List<Section> sections = new List<Section>()
    {
        { new Section() },
    };
}

public class Section
{
    public string name = "";
    public QuestionState state = QuestionState.Dialogue;
    public Dictionary<string, string> values = new Dictionary<string, string>()
    {
        {  "CharacterName#LineForCharacter", "DialogueText" },
    };
}

public enum AnswerType
{
    Short,
    Multi,
}