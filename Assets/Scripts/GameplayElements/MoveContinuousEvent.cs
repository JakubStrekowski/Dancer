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
    [SerializeField]
    private SpriteRenderer holdBar;

    private readonly float DESTROY_DELAY = 5.0f;

    private float fillPercentage = 0;
    private float moveSpeed = 10f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isCheckedCorrect = false;

    private float durationInSeconds;
    private float passedTimeWhileHeld;

    public bool IsBeingHeld { get; set; }
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
        if(!isCheckedCorrect && fillPercentage == 0)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
            IsBeingHeld = true;
            rb.velocity = Vector2.zero;
            passedTimeWhileHeld = 0;
        }
    }

    public void OnMoveEventMissed()
    {
        if(!(fillPercentage > 0))
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        }
        if(!isCheckedCorrect)
        {
            NoteChecker.OnButtonMistake();
        }
        //TODO here I can add some custom effects
        StartCoroutine(nameof(DestroyAfterTime));
    }

    public void SetObjectVals(float beginTime, float duration, MoveTypeEnum moveType, float ticksPerSpeed)
    {
        BeginTime = beginTime;
        Duration = duration;
        MoveType = moveType;
        holdBar.transform.localScale = new Vector3((1f * ticksPerSpeed / duration), transform.localScale.y);
        durationInSeconds = 1f * ticksPerSpeed / duration;
    }
    public void ActivateEvent(float speed)
    {
        moveSpeed = speed;
        rb.velocity = Vector2.left * moveSpeed;
        sr.enabled = true;
        holdBar.enabled = true;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(IsBeingHeld && fillPercentage < 1)
        {
            passedTimeWhileHeld += Time.deltaTime * 4;
            fillPercentage = (passedTimeWhileHeld / durationInSeconds);
            holdBar.transform.localPosition = new Vector3(-fillPercentage * durationInSeconds, holdBar.transform.localPosition.y);
        }
        if(fillPercentage >= 1 && !isCheckedCorrect)
        {
            StopHolding();
            isCheckedCorrect = true;
            NoteChecker.OnButtonCorrect();
        }
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
    public bool isEventCheckedCorrect()
    {
        return isCheckedCorrect;
    }

    public bool isEventHeldDown()
    {
        return IsBeingHeld;
    }

    public void StopHolding()
    {
        IsBeingHeld = false;
        rb.velocity = moveSpeed * Vector2.left;
    }
}
