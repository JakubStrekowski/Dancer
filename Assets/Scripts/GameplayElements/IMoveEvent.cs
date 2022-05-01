

using UnityEngine;

public interface IMoveEvent
{
    public bool isEventCheckedCorrect();
    public float GetBeginTime();
    public void OnCorrectButtonInCollision();
    public void OnMoveEventMissed();
    public void SetObjectVals(
        float beginTime, float duration, MoveTypeEnum moveType, 
        float ticksPerSpeed, Color color);
    public void SetColor(Color newColor);
    public void SetActivateEvent(bool value);
    public MoveTypeEnum GetEventTypeID();

    public bool isEventHeldDown();

    public void StopHolding();
}
