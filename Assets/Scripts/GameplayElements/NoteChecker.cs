using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteChecker : MonoBehaviour
{
    private List<IMoveEvent> moveEventsInChecker;
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
        //TODO replae it with proper input system
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            foreach(IMoveEvent moveEvent in moveEventsInChecker)
            {
                if(moveEvent.GetEventTypeID() == MoveTypeEnum.Down && !moveEvent.isEventCheckedCorrect())
                {
                    moveEvent.OnCorrectButtonInCollision();
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            foreach (IMoveEvent moveEvent in moveEventsInChecker)
            {
                if (moveEvent.GetEventTypeID() == MoveTypeEnum.Left && !moveEvent.isEventCheckedCorrect())
                {
                    moveEvent.OnCorrectButtonInCollision();
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            foreach (IMoveEvent moveEvent in moveEventsInChecker)
            {
                if (moveEvent.GetEventTypeID() == MoveTypeEnum.Right && !moveEvent.isEventCheckedCorrect())
                {
                    moveEvent.OnCorrectButtonInCollision();
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            foreach (IMoveEvent moveEvent in moveEventsInChecker)
            {
                if (moveEvent.GetEventTypeID() == MoveTypeEnum.Up && !moveEvent.isEventCheckedCorrect())
                {
                    moveEvent.OnCorrectButtonInCollision();
                    break;
                }
            }
        }
    }
}
