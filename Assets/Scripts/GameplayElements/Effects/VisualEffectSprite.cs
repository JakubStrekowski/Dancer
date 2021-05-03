using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectSprite : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer sr;

    float ticksPerSpeed;
    private Vector2 velocity = Vector2.zero;

    public void SetParameters(float ticksPerSpeed)
    {
        this.ticksPerSpeed = ticksPerSpeed;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void MoveTowardsLinear(Vector2 position, float duration)
    {
        if (duration > 0)
        {
            StartCoroutine(MoveLinearProcedure(position, duration / ticksPerSpeed));
        }
        else
        {
            transform.position = position;
        }
    }
    public void MoveTowardsDamping(Vector2 position, float duration)
    {
        if (duration > 0)
        {
            StartCoroutine(MoveDampingProcedure(position, duration / ticksPerSpeed));
        }
        else
        {
            transform.position = position;
        }
    }
    public void RotateLinear(float rotateDegree, float duration)
    {
        if (duration > 0)
        {
            StartCoroutine(RotateLinearProcedure(rotateDegree, duration / ticksPerSpeed));
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotateDegree);
        }
    }
    public void RotateArc(float rotateDegree, float duration)
    {
        if (duration > 0)
        {
            StartCoroutine(RotateArcProcedure(rotateDegree,  duration / ticksPerSpeed));
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotateDegree);
        }
    }

    public void ChangeColorLinear(Color endColor, float duration)
    {
        if(duration > 0)
        {
            StartCoroutine(ChangeColorLienarProcedure(endColor, duration / ticksPerSpeed));
        }
        else
        {
            sr.color = endColor;
        }
    }
    public void ChangeColorArc(Color endColor, float duration)
    {
        if (duration > 0)
        {
            StartCoroutine(ChangeColorArcProcedure(endColor, duration / ticksPerSpeed));
        }
        else
        {
            sr.color = endColor;
        }
    }

    private IEnumerator MoveLinearProcedure(Vector2 position, float duration)
    {
        float time = 0;

        float targetX = position.x - transform.position.x;
        float targetY = position.y - transform.position.y;

        float prevAddX = 0;
        float prevAddY = 0;

        while (time <= duration)
        {
            float t = time / duration;

            float resultX = Mathf.Lerp(0, targetX, t);
            float resultY = Mathf.Lerp(0, targetY, t);

            transform.position = new Vector2(transform.position.x + resultX - prevAddX, transform.position.y + resultY - prevAddY);

            prevAddX = resultX;
            prevAddY = resultY;


            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RotateLinearProcedure(float rotation, float duration)
    {
        float time = 0;

        float targetZ = rotation - transform.rotation.z;

        float prevAddZ = 0;

        while (time <= duration)
        {
            float t = time / duration;

            float resultZ = Mathf.Lerp(0, targetZ, t);

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + resultZ - prevAddZ);

            prevAddZ = resultZ;


            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + targetZ - prevAddZ);
    }

    private IEnumerator RotateArcProcedure(float rotation, float duration)
    {
        float time = 0;

        float targetZ = rotation - transform.rotation.z;

        float prevAddZ = 0;

        while (time <= duration)
        {
            float t = time / duration;
            t = Mathf.Sqrt(t);

            float resultZ = Mathf.Lerp(0, targetZ, t);

            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + resultZ- - prevAddZ);

            prevAddZ = resultZ;


            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + targetZ - prevAddZ);
    }

    private IEnumerator MoveDampingProcedure(Vector2 position, float duration)
    {
        float time = 0;

        while (time <= duration)
        {
            transform.position = Vector2.SmoothDamp(transform.position, position, ref velocity, duration);

            yield return null;
        }
    }

    private IEnumerator ChangeColorLienarProcedure(Color endColor, float duration)
    {
        float time = 0;
        //colors are made separately to make changing separate channels easy
        float targetA = endColor.a - sr.color.a;
        float targetR = endColor.r - sr.color.r;
        float targetG = endColor.g - sr.color.g;
        float targetB = endColor.b - sr.color.b;

        float prevA = 0;
        float prevR = 0;
        float prevG = 0;
        float prevB = 0;
        while (time <= duration)
        {
            float t = time / duration;

            float resultA = Mathf.Lerp(0, targetA, t);
            float resultR = Mathf.Lerp(0, targetR, t);
            float resultG = Mathf.Lerp(0, targetG, t);
            float resultB = Mathf.Lerp(0, targetB, t);

            sr.color = new Color(sr.color.r + resultR - prevR, sr.color.g + resultG - prevG, sr.color.b + resultB - prevB, sr.color.a + resultA - prevA);


            prevA = resultA;
            prevR = resultR;
            prevG = resultG;
            prevB = resultB;

            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ChangeColorArcProcedure(Color endColor, float duration)
    {
        float time = 0;
        //colors are made separately to make changing separate channels easy
        float targetA = endColor.a - sr.color.a;
        float targetR = endColor.r - sr.color.r;
        float targetG = endColor.g - sr.color.g;
        float targetB = endColor.b - sr.color.b;

        float prevA = 0;
        float prevR = 0;
        float prevG = 0;
        float prevB = 0;
        while (time <= duration)
        {
            float t = time / duration;

            t = Mathf.Sqrt(t);

            float resultA = Mathf.Lerp(0, targetA, t);
            float resultR = Mathf.Lerp(0, targetR, t);
            float resultG = Mathf.Lerp(0, targetG, t);
            float resultB = Mathf.Lerp(0, targetB, t);

            sr.color = new Color(sr.color.r + resultR - prevR, sr.color.g + resultG - prevG, sr.color.b + resultB - prevB, sr.color.a + resultA - prevA);


            prevA = resultA;
            prevR = resultR;
            prevG = resultG;
            prevB = resultB;

            time += Time.deltaTime;
            yield return null;
        }
    }


}
