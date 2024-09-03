using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEvent : MonoBehaviour
{
    private IFade controller;

    public void Awake()
    {
        controller = gameObject.GetComponentInParent<IFade>();
    }

    public void UpdateFadeState()
    {
        controller.FadeFinished();
    }
}