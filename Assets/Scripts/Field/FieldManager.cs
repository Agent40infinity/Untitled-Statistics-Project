using System.Collections;
using System.Collections.Generic;

public class FieldManager
{
    private static FieldState state = FieldState.Poverty;
    public static List<FieldState> complete = new List<FieldState>();
    public static int selector = 1;

    public static string GetState
    {
        get { return state.ToString(); }
    }

    public static FieldState State
    {
        get { return state; }
        set { state = value; }
    }

    public static bool CheckComplete
    {
        get { return System.Enum.GetValues(typeof(FieldState)).Length == complete.Count; }
    }

    public static FieldState Completed
    {
        set { complete.Add(value); }
    }
}

public enum FieldState
{ 
    Poverty,
    Education,
    Health,
}