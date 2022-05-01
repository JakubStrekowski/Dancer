using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInstantEvent : MonoBehaviour, IMoveEvent
{
    private readonly float DISABLE_DELAY = 5.0f;

    private SpriteRenderer sr;
    private bool isCheckedCorrect = false;
    private Color colorToSet;

    public ParticleSystem onHitEffect;
    public float BeginTime { get; private set; }
    public MoveTypeEnum MoveType { get; private set; }

    public void ActivateEvent(float speed)
    {
        sr.enabled = true;
        sr.color = colorToSet;
    }

    public float GetBeginTime()
    {
        return BeginTime;
    }

    public MoveTypeEnum GetEventTypeID()
    {
        return MoveType;
    }

    public bool isEventCheckedCorrect()
    {
        return isCheckedCorrect;
    }

    public void OnCorrectButtonInCollision()
    {
        isCheckedCorrect = true;
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        onHitEffect.Play();
        NoteChecker.OnButtonCorrect();
    }

    public void OnMoveEventMissed()
    {

        if (!isCheckedCorrect)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
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
        MoveType = moveType;
        colorToSet = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ParticleSystem.MainModule particleMainModule = onHitEffect.main;
        particleMainModule.startColor = colorToSet;
    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(DISABLE_DELAY);
        sr.enabled = false;
    }

    public bool isEventHeldDown()
    {
        return false;
    }

    public void StopHolding()
    {
        return;
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
