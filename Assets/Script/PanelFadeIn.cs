using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFadeIn : MonoBehaviour
{
    public Animator panelAnimator;


    public void FadeIn()
    {
        Debug.Log("fadein");
        panelAnimator.Play("PanelFadeIn");
    }
}
