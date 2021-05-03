using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteChecker : MonoBehaviour
{
    private List<IMoveEvent> moveEventsInChecker;

    public delegate void OnHitMistakeDelegate();
    public static OnHitMistakeDelegate hitMistakeDelegate;

    public delegate void OnHitCorrectDelegate();
    public static OnHitCorrectDelegate hitCorrectDelegate;

    // Start is called before the first frame update
    void Awake()
    {
        moveEventsInChecker = new List<IMoveEvent>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        moveEventsInChecker.Add(collision.gameObject.GetComponent("IMoveEvent") as IMoveEvent);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        moveEventsInChecker.Remove(other.gameObject.GetComponent("IMoveEvent") as IMoveEvent);
        (other.gameObject.GetComponent("IMoveEvent") as IMoveEvent).OnMoveEventMissed();
    }

    void Update()
    {
        bool hitCorrect = false;
        //TODO replace it with proper input system
        if (Input.GetButtonDown("Down"))
        {
            foreach (IMoveEvent moveEvent in moveEventsInChecker)
            {
                if (moveEvent.GetEventTypeID() == MoveTypeEnum.Down && !moveEvent.isEventCheckedCorrect())
                {
                    moveEvent.OnCorrectButtonInCollision();
                    hitCorrect = true;
                    break;
                }
            }
            if (!hitCorrect) OnButtonMistake();
        }
        if (Input.GetButtonDown("Left"))
        {
            foreach (IMoveEvent moveEvent in moveEventsInChecker)
            {
                if (moveEvent.GetEventTypeID() == MoveTypeEnum.Left && !moveEvent.isEventCheckedCorrect())
                {
                    moveEvent.OnCorrectButtonInCollision();
                    hitCorrect = true;
                    break;
                }
            }
            if (!hitCorrect) OnButtonMistake();
        }
        if (Input.GetButtonDown("Right"))
        {
            foreach (IMoveEvent moveEvent in moveEventsInChecker)
            {
                if (moveEvent.GetEventTypeID() == MoveTypeEnum.Right && !moveEvent.isEventCheckedCorrect())
                {
                    moveEvent.OnCorrectButtonInCollision();
                    hitCorrect = true;
                    break;
                }
            }
            if (!hitCorrect) OnButtonMistake();
        }
        if (Input.GetButtonDown("Up"))
        {
            foreach (IMoveEvent moveEvent in moveEventsInChecker)
            {
                if (moveEvent.GetEventTypeID() == MoveTypeEnum.Up && !moveEvent.isEventCheckedCorrect())
                {
                    moveEvent.OnCorrectButtonInCollision();
                    hitCorrect = true;
                    break;
                }
            }
            if (!hitCorrect) OnButtonMistake();
        }
        //---------------------


        if (Input.GetButtonUp("Down"))
        {

            foreach (IMoveEvent moveEvent in moveEventsInChecker)
            {
                if (moveEvent.GetEventTypeID() == MoveTypeEnum.Down && moveEvent.isEventHeldDown())
                {
                    moveEvent.StopHolding();
                    break;
                }
            }
        }

        if (Input.GetButtonUp("Left"))
        {

            foreach (IMoveEvent moveEvent in moveEventsInChecker)
            {
                if (moveEvent.GetEventTypeID() == MoveTypeEnum.Left && moveEvent.isEventHeldDown())
                {
                    moveEvent.StopHolding();
                    break;
                }
            }
        }

        if (Input.GetButtonUp("Right"))
        {

            foreach (IMoveEvent moveEvent in moveEventsInChecker)
            {
                if (moveEvent.GetEventTypeID() == MoveTypeEnum.Right && moveEvent.isEventHeldDown())
                {
                    moveEvent.StopHolding();
                    break;
                }
            }
        }

        if (Input.GetButtonUp("Up"))
        {

            foreach (IMoveEvent moveEvent in moveEventsInChecker)
            {
                if (moveEvent.GetEventTypeID() == MoveTypeEnum.Up && moveEvent.isEventHeldDown())
                {
                    moveEvent.StopHolding();
                    break;
                }
            }
        }
    }
    public static void OnButtonMistake()
    {
        hitMistakeDelegate();
    }
    public static void OnButtonCorrect()
    {
        hitCorrectDelegate();
    }
}
