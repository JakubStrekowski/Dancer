using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private readonly float ADDITIONAL_START_TIME = 5.0f;

    List<GameObject> moveEvents;
    private float currentMusicTime = 0.0f;

    public float moveSpeed = 10f;

    public MoveFactory moveFactory;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        //TODO set another stage in loader
        moveEvents = moveFactory.GenerateGameMovesFromXml(GameMaster.Instance.musicLoader.DancerEvents);
        
        currentMusicTime = 0 - ADDITIONAL_START_TIME;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentMusicTime += Time.deltaTime;
        //TODO this needs to be fixed
        if (moveEvents.Count != 0)
        {
            //for (int i = 0; i < moveEvents.Count; i++)
            {
                if ((moveEvents[0].GetComponent("IMoveEvent") as IMoveEvent).GetBeginTime() - 100 < currentMusicTime)
                {
                    moveEvents[0].SetActive(true);
                    moveEvents.RemoveAt(0);
                    //i--;
                    //continue;
                }
                else
                {
                    //break;
                }
            }
        }
    }
}
