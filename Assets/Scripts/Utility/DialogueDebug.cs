using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueDebug
{
    public static string CheckDialogue(ProcessedDialogue input)
    {
        string output = "DC: " + input.title.Count + " | TC: " + input.expression.Count + " | DC: " + input.delay.Count + " | BC: " + input.background.Count + " | PC: " + input.position.Count;
        output += System.Environment.NewLine;

        for (int i = 0; i < input.dialogue.Count; i++)
        {
            string text = "";
            string expression = "";
            string position = "";
            string background = "";

            if (input.dialogue[i] != null || input.dialogue[i] != "")
            {
                text = "Exists";
            }
            else
            {
                text = "None";
            }


            if (input.expression[i] != null || input.expression[i] != "")
            {
                expression = input.expression[i];
            }
            else

            /*if (input.position[i] != null)
            {*/
                position = input.position[i].ToString();
            //}

            if (input.background[i] != null || input.background[i] != "")
            {
                background = input.background[i];
            }

            output += "Char: " + input.title[i] +" | Dialogue: " + text + " | Delay: " + input.delay[i] + " | Expression: " + expression + " | Position: " + position + " | Background: " + background;
            output += System.Environment.NewLine;
        }


        return output;
    }
}
