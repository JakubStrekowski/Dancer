using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIDIparser.Models;


public class MoveFactory : MonoBehaviour
{
    private readonly float[] eventYPositions = { 7.6f, 6.9f, 6.2f, 5.5f };
    private readonly float EVENT_STARTPOS_X = 14f;

    private Color[] moveColors = { 
        ArgbColor.ConvertFromBytes(255, 185, 255, 255),
        ArgbColor.ConvertFromBytes(255, 63, 255, 13),
        ArgbColor.ConvertFromBytes(255, 162, 0, 255),
        ArgbColor.ConvertFromBytes(255, 255, 173, 0)
    };

    public GameObject uiAnchor;
    public GameObject moveContinuousEventBase;
    public GameObject moveInstantEventBase;

    public List<GameObject> GenerateGameMovesFromXml(DancerEvents dancerEvents, float ticksPerSpeed)
    {
        List<GameObject> createdMoveEvents = new List<GameObject>();
        foreach(MusicMovementEvent movementEvent in dancerEvents.movementEvents)
        {
            MoveTypeEnum moveTypeEnum;
            Vector3 eventPosition = new Vector3(EVENT_STARTPOS_X, 8.1f,0);
            //if event is move contiuous
            if (movementEvent.EventTypeID > EventTypeEnum.ArrowDownInstant && movementEvent.EventTypeID < EventTypeEnum.ChangeBackground)
            {
                GameObject newMoveEvent = Instantiate(moveContinuousEventBase, uiAnchor.transform).gameObject;
                switch (movementEvent.EventTypeID)
                {
                    case EventTypeEnum.ArrowUpDuration:    moveTypeEnum = MoveTypeEnum.Up; eventPosition.y = eventYPositions[0];     break;
                    case EventTypeEnum.ArrowRightDuration: moveTypeEnum = MoveTypeEnum.Right; eventPosition.y = eventYPositions[1];  break;
                    case EventTypeEnum.ArrowLeftDuration:  moveTypeEnum = MoveTypeEnum.Left; eventPosition.y = eventYPositions[2];   break;
                    case EventTypeEnum.ArrowDownDuration:  moveTypeEnum = MoveTypeEnum.Down; eventPosition.y = eventYPositions[3];   break;
                    default: moveTypeEnum = MoveTypeEnum.Down; eventPosition.y = eventYPositions[3]; break;
                }
                newMoveEvent.GetComponent<IMoveEvent>().SetObjectVals(movementEvent.StartTime, movementEvent.Duration, moveTypeEnum, ticksPerSpeed, moveColors[(int)moveTypeEnum]);
                newMoveEvent.transform.position = eventPosition;
                createdMoveEvents.Add(newMoveEvent);
            }
            //if event is move instant
            else if (movementEvent.EventTypeID <= EventTypeEnum.ArrowDownInstant)
            {
                GameObject newMoveEvent = Instantiate(moveInstantEventBase, uiAnchor.transform).gameObject;
                switch (movementEvent.EventTypeID)
                {
                    case EventTypeEnum.ArrowUpInstant:    moveTypeEnum = MoveTypeEnum.Up; eventPosition.y = eventYPositions[0];    break;
                    case EventTypeEnum.ArrowRightInstant: moveTypeEnum = MoveTypeEnum.Right; eventPosition.y = eventYPositions[1]; break;
                    case EventTypeEnum.ArrowLeftInstant:  moveTypeEnum = MoveTypeEnum.Left; eventPosition.y = eventYPositions[2];  break;
                    case EventTypeEnum.ArrowDownInstant:  moveTypeEnum = MoveTypeEnum.Down; eventPosition.y = eventYPositions[3];  break;
                    default: moveTypeEnum = MoveTypeEnum.Down; break;
                }
                newMoveEvent.GetComponent<IMoveEvent>().SetObjectVals(movementEvent.StartTime, movementEvent.Duration, moveTypeEnum, ticksPerSpeed, moveColors[(int)moveTypeEnum]);
                newMoveEvent.transform.position = eventPosition;
                createdMoveEvents.Add(newMoveEvent);
            }
        }
        return createdMoveEvents;
    }

    public void SetMoveColor(int colorID, Color color)
    {
        moveColors[colorID] = color;
    }
}
