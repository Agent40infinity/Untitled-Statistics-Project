using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSelection : MonoBehaviour
{
    public List<GameObject> options;
    public List<List<float>> position = new List<List<float>>
    {
        { new List<float> { 0 } },
        { new List<float> { -120, 120 } },
        { new List<float> { -234, 0, 234 } },
    };

    public FadeController fade;

    public void Awake()
    {
        fade = GameObject.FindWithTag("FadeController").GetComponent<FadeController>();

        for (int i = 0; i < FieldManager.complete.Count; i++)
        {
            switch (FieldManager.complete[i])
            {
                case FieldState.Poverty:
                    options[0].SetActive(false);
                    options.RemoveAt(0);
                    break;
                case FieldState.Education:
                    options[1].SetActive(false);
                    options.RemoveAt(1);
                    break;
                case FieldState.Health:
                    options[2].SetActive(false);
                    options.RemoveAt(2);
                    break;
            }
        }

        for (int j = 0; j < options.Count; j++)
        {
            options[j].transform.localPosition = new Vector2(position[options.Count - 1][j], 0);
        }
    }

    public void SelectField(int index)
    {
        FieldManager.State = (FieldState)System.Enum.GetValues(typeof(FieldState)).GetValue(index);
        StartCoroutine(NextLevel());
    }

    public IEnumerator NextLevel()
    {
        yield return fade.FadeOut();
        GameManager.instance.SwapLevel();
    }
}
