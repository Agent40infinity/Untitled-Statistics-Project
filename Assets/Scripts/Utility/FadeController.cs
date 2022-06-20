using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public Animator fade; //Creates a reference for the Animator: Fade.
    public FadeState fadeState;

    public IEnumerator FadeOut(string name) //Called upon to Fade Out.
    {
        Debug.Log("Fade Out from " + name);
        fade.SetTrigger("FadeOut");
        yield return FadeWait();
        Debug.Log("Fade Out from Finished");
    }

    public void FadeIn() //Called upon to Fade In.
    {
        Debug.Log("Fade in");
        fade.SetTrigger("FadeIn");
    }

    public IEnumerator FadeWait()
    {
        while (!FinishedFade())
        {
            Debug.Log("Waiting");
            yield return null;
        }
    }

    public bool FinishedFade()
    {
        switch (fadeState)
        {
            case FadeState.Ended:
                fadeState = FadeState.Idle;
                return true;
            default:
                return false;
        }
    }
}

public enum FadeState
{
    Idle,
    Ended
}
