using MIDIparser.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MoveTypeEnum
{
    Up,
    Right,
    Left,
    Down
}

public class MoveContinuousEvent : MonoBehaviour, IMoveEvent
{
    private readonly float DESTROY_DELAY = 5.0f;

    private float fillPercentage = 0;
    private float moveSpeed = 10f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public float MoveSpeed { get => moveSpeed; private set => moveSpeed=value; }
    public float BeginTime { get; private set; }
    public float Duration { get; private set; }
    public MoveTypeEnum MoveType { get; private set; }

    public float GetBeginTime()
    {
        return BeginTime;
    }

    public void OnCorrectButtonInCollision()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
        fillPercentage = 100;
    }

    public void OnMoveEventMissed()
    {
        if(!(fillPercentage > 0))
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        }
        //TODO here I can add some custom effects
        StartCoroutine(nameof(DestroyAfterTime));
    }

    public void SetObjectVals(float beginTime, float duration, MoveTypeEnum moveType)
    {
        BeginTime = beginTime;
        Duration = duration;
        MoveType = moveType;
        transform.localScale = new Vector3((transform.localScale.x * duration/1000) + 1, transform.localScale.y);
    }
    public void ActivateEvent(float speed)
    {
        moveSpeed = speed;
        rb.velocity = Vector2.left * moveSpeed;
        sr.enabled = true;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(DESTROY_DELAY);
        Destroy(gameObject); // this is for 2 second delay
    }

    public MoveTypeEnum GetEventTypeID()
    {
        return MoveType;
    }
}
