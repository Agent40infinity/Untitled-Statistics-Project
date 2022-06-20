using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollection : MonoBehaviour
{
    public TMP_InputField firstName;
    public TMP_InputField lastName;
    public TMP_InputField id;
    public GameObject warning;

    public FadeController fade;

    public void Awake()
    {
        fade = GameObject.FindWithTag("FadeController").GetComponent<FadeController>();
    }

    public void SubmitData()
    {
        Debug.Log(firstName.text.Length);
        /*if (firstName.text.Length > 0 && lastName.text.Length > 0 && id.text.Length > 0)
        {
            GameManager.playerData.firstName = firstName.text;
            GameManager.playerData.lastName = lastName.text;
            GameManager.playerData.playerID = int.Parse(id.text);

            StartCoroutine(NextLevel());
        }
        else
        {
            warning.SetActive(true);
        }*/
    }

    /*public IEnumerator NextLevel()
    {
        yield return fade.FadeOut();
        GameManager.instance.SwapLevel();
    }*/
}
