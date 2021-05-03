using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInstantEvent : MonoBehaviour, IMoveEvent
{
    private readonly float DESTROY_DELAY = 5.0f;

    private float moveSpeed = 10f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isCheckedCorrect = false;
    private Color colorToSet;

    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }
    public float BeginTime { get; private set; }
    public MoveTypeEnum MoveType { get; private set; }

    public void ActivateEvent(float speed)
    {
        moveSpeed = speed;
        rb.velocity = Vector2.left * moveSpeed;
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
        StartCoroutine(nameof(DestroyAfterTime));
    }

    public void SetObjectVals(float beginTime, float duration, MoveTypeEnum moveType, float ticksPerSpeed, Color color)
    {
        BeginTime = beginTime;
        MoveType = moveType;
        colorToSet = color;
    }

    // Start is called before the first frame update
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

    public bool isEventHeldDown()
    {
        return false;
    }

    public void StopHolding()
    {
        return;
    }
}
