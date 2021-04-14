using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIDIparser.Models;


public class MoveFactory : MonoBehaviour
{
    private readonly float[] eventYPositions = { 8.1f, 7.4f, 6.7f, 6f };
    private readonly float EVENT_STARTPOS_X = 14f;

    public GameObject moveContinuousEventBase;
    public GameObject moveInstantEventBase;

    public List<GameObject> GenerateGameMovesFromXml(DancerEvents dancerEvents)
    {
        List<GameObject> createdMoveEvents = new List<GameObject>();
        foreach(MusicMovementEvent movementEvent in dancerEvents.movementEvents)
        {
            MoveTypeEnum moveTypeEnum;
            Vector3 eventPosition = new Vector3(EVENT_STARTPOS_X, 8.1f,0);
            //if event is move contiuous
            if (movementEvent.EventTypeID > EventTypeEnum.ArrowDownInstant && movementEvent.EventTypeID < EventTypeEnum.ChangeBackground)
            {
                GameObject newMoveEvent = Instantiate(moveContinuousEventBase).gameObject;
                switch (movementEvent.EventTypeID)
                {
                    case EventTypeEnum.ArrowUpDuration:    moveTypeEnum = MoveTypeEnum.Up; eventPosition.y = eventYPositions[0];     break;
                    case EventTypeEnum.ArrowRightDuration: moveTypeEnum = MoveTypeEnum.Right; eventPosition.y = eventYPositions[1];  break;
                    case EventTypeEnum.ArrowLeftDuration:  moveTypeEnum = MoveTypeEnum.Left; eventPosition.y = eventYPositions[2];   break;
                    case EventTypeEnum.ArrowDownDuration:  moveTypeEnum = MoveTypeEnum.Down; eventPosition.y = eventYPositions[3];   break;
                    default: moveTypeEnum = MoveTypeEnum.Down; eventPosition.y = eventYPositions[3]; break;
                }
                newMoveEvent.GetComponent<IMoveEvent>().SetObjectVals(movementEvent.StartTime, movementEvent.Duration, moveTypeEnum);
                newMoveEvent.transform.position = eventPosition;
                createdMoveEvents.Add(newMoveEvent);
            }
            //if event is move instant
            else if (movementEvent.EventTypeID <= EventTypeEnum.ArrowDownInstant)
            {
                GameObject newMoveEvent = Instantiate(moveInstantEventBase).gameObject;
                switch (movementEvent.EventTypeID)
                {
                    case EventTypeEnum.ArrowUpInstant:    moveTypeEnum = MoveTypeEnum.Up; eventPosition.y = eventYPositions[0];    break;
                    case EventTypeEnum.ArrowRightInstant: moveTypeEnum = MoveTypeEnum.Right; eventPosition.y = eventYPositions[1]; break;
                    case EventTypeEnum.ArrowLeftInstant:  moveTypeEnum = MoveTypeEnum.Left; eventPosition.y = eventYPositions[2];  break;
                    case EventTypeEnum.ArrowDownInstant:  moveTypeEnum = MoveTypeEnum.Down; eventPosition.y = eventYPositions[3];  break;
                    default: moveTypeEnum = MoveTypeEnum.Down; break;
                }
                newMoveEvent.GetComponent<IMoveEvent>().SetObjectVals(movementEvent.StartTime, movementEvent.Duration, moveTypeEnum);
                newMoveEvent.transform.position = eventPosition;
                createdMoveEvents.Add(newMoveEvent);
            }
        }
        return createdMoveEvents;
    }
}
