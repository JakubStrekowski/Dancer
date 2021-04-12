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

    private readonly float ADDITIONAL_START_TIME = 6f;
    private readonly float SPAWN_DISTANCE = 10;
    private readonly float CONST_SPEED_MODIFIER = 0.40f;

    List<GameObject> moveEvents;
    private float currentMusicTime = 0.0f;

    private float speedDiffficulty = 1.0f;
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
        moveSpeed = SPAWN_DISTANCE * CONST_SPEED_MODIFIER * speedDiffficulty;
        //TODO set another stage in loader
        moveEvents = moveFactory.GenerateGameMovesFromXml(GameMaster.Instance.musicLoader.DancerSongParsed.dancerEvents);

        StartCoroutine(CheckSongMovesLoaded());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (songState == ESongStates.Loading) return;
        if (songState == ESongStates.ReadyToPlay)
        {
            audioSrc.clip = currentSong;
            currentMusicTime = 0;
            songState = ESongStates.Playing;
            return;
        }
        if(songState == ESongStates.Playing)
        {
            currentMusicTime += Time.deltaTime;

            if(currentMusicTime > ADDITIONAL_START_TIME && audioSrc.isPlaying == false) audioSrc.Play();

            //TODO this needs to be fixed
            if (moveEvents.Count != 0)
            {
                //for (int i = 0; i < moveEvents.Count; i++)
                {
                    if (((moveEvents[0].GetComponent("IMoveEvent") as IMoveEvent).GetBeginTime() * 5) < currentMusicTime * 1000)
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
