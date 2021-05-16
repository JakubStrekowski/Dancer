using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ArrowLightEffect : MonoBehaviour
{
    static private readonly float LIGHT_INTENSITY_LOW = 0.5f;
    static private readonly float LIGHT_INTENSITY_HIGH = 1f;

    static private readonly float REDUCE_LIGHT_SPEED = 2f;

    private Light2D myLight;

    private void Awake()
    {
        myLight = GetComponent<Light2D>();
    }

    private void Start()
    {
        StartCoroutine(ReduceLight());
    }

    private IEnumerator ReduceLight()
    {
        while (true)
        {
            if(myLight.intensity > LIGHT_INTENSITY_LOW)
            {
                myLight.intensity = (myLight.intensity - Time.deltaTime * REDUCE_LIGHT_SPEED) > LIGHT_INTENSITY_LOW ? 
                    myLight.intensity - Time.deltaTime * REDUCE_LIGHT_SPEED : LIGHT_INTENSITY_LOW;
            }
            yield return null;
        }
    }

    public void LightUpEvent()
    {
        myLight.intensity = LIGHT_INTENSITY_HIGH;
    }
}
