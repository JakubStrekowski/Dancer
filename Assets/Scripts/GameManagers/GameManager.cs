using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ESongStates
{
    Loading,
    ReadyToPlay,
    Playing,
    Finished,
    Finalized
}

public class GameManager : MonoBehaviour
{
    private AudioClip currentSong;
    private readonly int MAX_LOAD_TRIES = 5;

    private readonly float DISTANCE_TO_CHECKER = 20f;
    private readonly float BASE_SPEED = 4; //with this speed event will reach checker in 5 second
    private float timeToReachChecker = 5f;
    private float tickPerSecond = 1680f;

    List<GameObject> moveEvents;
    private float currentMusicTime = 0.0f;

    private float speedDiffficulty = 1.0f; //scaling BASE_SPEED to make lever harder
    public float moveSpeed = 10;

    public MoveFactory moveFactory;
    public AudioSource audioSrc;

    ESongStates songState = ESongStates.Loading;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = BASE_SPEED * speedDiffficulty;
        timeToReachChecker = DISTANCE_TO_CHECKER / moveSpeed;
        //TODO set another stage in loader
        moveEvents = moveFactory.GenerateGameMovesFromXml(GameMaster.Instance.musicLoader.DancerSongParsed.dancerEvents);

        StartCoroutine(CheckSongMovesLoaded());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(songState)
        {
            case ESongStates.Loading:
                {
                    return;
                }
            case ESongStates.ReadyToPlay:
                {
                    audioSrc.clip = currentSong;
                    currentMusicTime = 0;
                    songState = ESongStates.Playing;
                    return;
                }
            case ESongStates.Playing:
                {
                    currentMusicTime += Time.deltaTime;

                    if (currentMusicTime > timeToReachChecker && audioSrc.isPlaying == false)
                    {
                        songState = ESongStates.Playing;
                        audioSrc.Play();
                    }

                    if (moveEvents.Count != 0)
                    {
                        //for (int i = 0; i < moveEvents.Count; i++)
                        {
                            if (((moveEvents[0].GetComponent("IMoveEvent") as IMoveEvent).GetBeginTime() / tickPerSecond) < currentMusicTime)
                            {
                                (moveEvents[0].GetComponent("IMoveEvent") as IMoveEvent).ActivateEvent(moveSpeed);
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
                    break;
                }
        }
    }

    IEnumerator CheckSongMovesLoaded()
    {
        int tries = 0;
        while(tries < MAX_LOAD_TRIES)
        {
            if (currentSong is null || moveEvents is null)
            {
                tries++;
                yield return new WaitForSeconds(1);
                currentSong = GameMaster.Instance.nextMusic;
            }
            else
            {
                songState = ESongStates.ReadyToPlay;
                yield break;
            }
        }
        if (currentSong is null || moveEvents is null)
        {
            tries++;
        }
    }
}
