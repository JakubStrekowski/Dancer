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

    private readonly float DISABLE_DELAY = 5.0f;

    private float fillPercentage = 0;
    private SpriteRenderer sr;
    private bool isCheckedCorrect = false;
    private bool isReleasedTooEarly = false;
    private Color colorToSet;

    private float durationInSeconds;
    private float passedTimeWhileHeld;
    private float originalPosX = 0;

    public ParticleSystem onHitEffect;
    public bool IsBeingHeld { get; set; }
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
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            IsBeingHeld = true;
            passedTimeWhileHeld = 0;
            onHitEffect.Play();
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
        StartCoroutine(nameof(DisableAfterTime));
    }

    public void SetObjectVals(
        float beginTime, float duration, 
        MoveTypeEnum moveType, float ticksPerSpeed, Color color)
    {
        BeginTime = beginTime;
        Duration = duration;
        MoveType = moveType;
        holdBar.transform.localScale = new Vector3(
            1f * duration / ticksPerSpeed, 
            transform.localScale.y);

        durationInSeconds = 1f * duration / ticksPerSpeed;
        colorToSet = color;
    }
    public void SetActivateEvent(bool value)
    {
        if (value == true)
        {
            originalPosX = transform.localPosition.x;
        }
        else
        {
            StopCoroutine(nameof(DisableAfterTime));

            fillPercentage = 0;
            isCheckedCorrect = false;
            isReleasedTooEarly = false;

            sr.color = colorToSet;
            ParticleSystem.MainModule particleMainModule = onHitEffect.main;
            particleMainModule.startColor = colorToSet;

            holdBar.transform.localPosition = Vector3.zero;
            transform.localPosition = new Vector3(
                originalPosX,
                transform.localPosition.y,
                transform.localPosition.z);
        }

        sr.enabled = value;
        holdBar.enabled = value;

    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ParticleSystem.MainModule particleMainModule = onHitEffect.main;
        particleMainModule.startColor = colorToSet;
    }

    void Update()
    {
        if(IsBeingHeld && fillPercentage < 1)
        {
            passedTimeWhileHeld += Time.deltaTime * Constants.BASE_SPEED;
            fillPercentage = (passedTimeWhileHeld / durationInSeconds);

            transform.localPosition = new Vector3(
                originalPosX + fillPercentage * durationInSeconds,
                transform.localPosition.y);

            holdBar.transform.localPosition = new Vector3(
                -fillPercentage * durationInSeconds, 
                holdBar.transform.localPosition.y);
        }
        if(fillPercentage >= 1 && !isCheckedCorrect)
        {
            StopHolding();
            isCheckedCorrect = true;
            NoteChecker.OnButtonCorrect();
        }
    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(DISABLE_DELAY);

        sr.enabled = false;
        holdBar.enabled = false;
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
        if(fillPercentage < 1)
        {
            isReleasedTooEarly = true;
        }
    }

    public bool IsReleasedTooEarly()
    {
        return isReleasedTooEarly;
    }

    public void SetColor(Color newColor)
    {
        sr = GetComponent<SpriteRenderer>();
        colorToSet = newColor;
        sr.color = colorToSet;
        ParticleSystem.MainModule particleMainModule = onHitEffect.main;
        particleMainModule.startColor = colorToSet;
    }
}
