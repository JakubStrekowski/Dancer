using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class NoteChecker : MonoBehaviour
{
    private readonly float LIGHT_INTENSITY_LOW = 0.5f;
    private readonly float LIGHT_INTENSITY_HIGH = 1f;

    private List<IMoveEvent> moveEventsInChecker;
    private float timeToRestart = 1f;
    private float timeRestartPassed = 0;

    public delegate void OnHitMistakeDelegate();
    public static OnHitMistakeDelegate hitMistakeDelegate;

    public delegate void OnHitCorrectDelegate();
    public static OnHitCorrectDelegate hitCorrectDelegate;

    public Light2D[] arrowLights;
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
            arrowLights[(int)MoveTypeEnum.Down].intensity = LIGHT_INTENSITY_HIGH;

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
            arrowLights[(int)MoveTypeEnum.Left].intensity = LIGHT_INTENSITY_HIGH;

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
            arrowLights[(int)MoveTypeEnum.Right].intensity = LIGHT_INTENSITY_HIGH;

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
            arrowLights[(int)MoveTypeEnum.Up].intensity = LIGHT_INTENSITY_HIGH;

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

            arrowLights[(int)MoveTypeEnum.Down].intensity = LIGHT_INTENSITY_LOW;
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

            arrowLights[(int)MoveTypeEnum.Left].intensity = LIGHT_INTENSITY_LOW;
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

            arrowLights[(int)MoveTypeEnum.Right].intensity = LIGHT_INTENSITY_LOW;
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

            arrowLights[(int)MoveTypeEnum.Up].intensity = LIGHT_INTENSITY_LOW;
        }

        //--resetting song
        if(Input.GetButton("Restart"))
        {
            timeRestartPassed += Time.deltaTime;
            if(timeRestartPassed >= timeToRestart)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if(Input.GetButtonUp("Restart"))
        {
            timeRestartPassed = 0;
        }

        //escaping to menu
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene((int)ESceneIndexes.mainMenuSceneIndex);
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
