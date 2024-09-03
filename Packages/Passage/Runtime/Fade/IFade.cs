using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFade
{
    IEnumerator FadeIn();
    IEnumerator FadeOut();
    void FadeFinished();
}
