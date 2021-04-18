using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SetCustomArrowColor : MonoBehaviour
{
    public SpriteRenderer sr;
    public Light2D light2D;

    public void SetColor(Color color)
    {
        sr.color = color;
        light2D.color = color;
    }
}
