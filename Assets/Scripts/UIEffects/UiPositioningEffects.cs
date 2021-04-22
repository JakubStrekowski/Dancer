using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPositioningEffects : MonoBehaviour
{
    private Vector2 startPosiion;
    private Vector2 endposition;

    //TODO probably need to rebuild the notefactory to create events lazily if i want to kee this
    bool floatingEnabled = false;

    void Start()
    {
        startPosiion = new Vector2(transform.position.x, transform.position.y);
        endposition = new Vector2(transform.position.x, transform.position.y - 1);
    }

    private void Update()
    {
        if(floatingEnabled)
        {
            if (transform.position.y == startPosiion.y)
            {
                StartCoroutine(LerpFunction(endposition.y, 5));
            }
            else if (transform.position.y == endposition.y)
            {
                StartCoroutine(LerpFunction(startPosiion.y, 5));
            }
        }
    }

    IEnumerator LerpFunction(float endValue, float duration)
    {
        float time = 0;
        float startValue = transform.position.y;

        while (time < duration)
        {
            float t = time / duration;

            t = t * t * (3f - 2f * t);

            float result = Mathf.Lerp(startValue, endValue, t);

            transform.position = new Vector2(transform.position.x, result);

            time += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector2(transform.position.x, endValue);
    }

    public void EnableFloating()
    {
        floatingEnabled = true;
    }

}
