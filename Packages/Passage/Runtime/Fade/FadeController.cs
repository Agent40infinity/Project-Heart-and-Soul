using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour, IFade
{
    private enum FadeState { Idle, Ended }

    public Animator fade; //Creates a reference for the Animator: Fade.
    private FadeState fadeState = FadeState.Idle;

    public void FadeFinished()
        => fadeState = FadeState.Ended;

    public IEnumerator FadeIn() //Called upon to Fade Out.
    {
        fade.SetTrigger("FadeIn");
        yield return FadeWait();
    }

    public IEnumerator FadeOut() //Called upon to Fade In.
    {
        fade.SetTrigger("FadeOut");
        yield return FadeWait();
    }

    private IEnumerator FadeWait()
    {
        while (!FinishedFade())
        {
            yield return null;
        }
    }

    private bool FinishedFade()
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