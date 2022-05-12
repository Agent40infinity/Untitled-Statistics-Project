using System.Collections;
using System.Collections.Generic;

public class FieldManager
{
    private static FieldState state = FieldState.Poverty;

    public static string GetState
    {
        get { return state.ToString(); }
    }

    public static FieldState SetState
    { 
        set { state = value; }
    }
}

public enum FieldState
{ 
    Poverty,
    Education,
    Health,
}