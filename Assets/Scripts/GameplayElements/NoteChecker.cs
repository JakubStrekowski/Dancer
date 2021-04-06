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

}
