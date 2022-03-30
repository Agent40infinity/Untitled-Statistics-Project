using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollection : MonoBehaviour
{
    public TMP_InputField player;
    public TMP_InputField id;

    public FadeController fade;

    public void Awake()
    {
        fade = GameObject.FindWithTag("FadeController").GetComponent<FadeController>();
    }

    public void SubmitData()
    {
        if (player.text != null && id.text != null)
        {
            GameManager.playerData.playerName = player.text;
            GameManager.playerData.playerID = int.Parse(id.text);

            StartCoroutine(NextLevel());
        }
    }

    public IEnumerator NextLevel()
    {
        yield return fade.FadeOut();
        GameManager.instance.SwapLevel();
    }
}
