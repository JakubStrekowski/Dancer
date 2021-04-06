

public interface IMoveEvent
{
    public float GetBeginTime();
    public void OnCorrectButtonInCollision();
    public void OnMoveEventMissed();
    public void SetObjectVals(float beginTime, float duration, MoveTypeEnum moveType);
}
