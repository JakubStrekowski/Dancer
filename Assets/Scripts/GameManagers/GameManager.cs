using MIDIparser.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ESongStates
{
    AwaitingToStart,
    Loading,
    ReadyToPlay,
    Playing,
    Finished,
    Finalized
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIEffectsManager uiFxManager;

    [SerializeField]
    private UILogicManager uILogicManager;

    private AudioClip currentSong;
    private float timeToReachChecker = 5f;
    private float tickPerSecond = 1680f;

    List<GameObject> moveEvents;
    List<VisualEventBase> visualEvents;

    private float currentMusicTime = 0.0f;

    private float speedDiffficulty = 1.0f; //scaling BASE_SPEED to make lever harder
    public float moveSpeed = 10;

    private int mistakeCount = 0;
    private int correctCount = 0;
    private int totalMoveEvents = 0;

    private float timeStampToFinish;

    public EffectsFactory effectsFactory;
    public MoveFactory moveFactory;
    public AudioSource audioSrc;

    private bool songWasPlayed = false;

    ESongStates songState = ESongStates.AwaitingToStart;
    private void Awake()
    {
        NoteChecker.hitMistakeDelegate += AddOneMistake;
        NoteChecker.hitCorrectDelegate += AddOneCorrect;
    }

    void OnEnable()
    {
        ReinitAndPlay();
    }

    private void OnDisable()
    {
        foreach(GameObject moveEventObject in moveEvents)
        {
            Destroy(moveEventObject);
        }

        effectsFactory.DeleteAllVisualSprites();

        songState = ESongStates.AwaitingToStart;

        audioSrc.Stop();
        songWasPlayed = false;
    }

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
                        if(songWasPlayed) //finishing song condiion
                        {
                            songState = ESongStates.Finished;
                            timeStampToFinish = currentMusicTime + Constants.TIME_AFTER_SONG_AWAIT;
                            break;
                        }

                        audioSrc.Play();
                        songWasPlayed = true;
                    }

                    uILogicManager.UpdateProgress(currentMusicTime);

                    //setting move events active when they reach (time - time for them to reach note checker)
                    if (moveEvents.Count != 0)
                    {
                        while(((moveEvents[0].GetComponent("IMoveEvent") as IMoveEvent).GetBeginTime() / tickPerSecond) < currentMusicTime)
                        {
                            (moveEvents[0].GetComponent("IMoveEvent") as IMoveEvent).ActivateEvent(moveSpeed);
                            moveEvents.RemoveAt(0);
                            if (moveEvents.Count == 0) break;
                        }
                    }

                    //setting visual events active when they reach their time
                    if (visualEvents.Count != 0)
                    {
                        while((visualEvents[0].startTime / tickPerSecond + timeToReachChecker) < currentMusicTime)
                        {
                            effectsFactory.ResolveEffect(visualEvents[0]);
                            visualEvents.RemoveAt(0);
                            if (visualEvents.Count == 0) break;
                        }
                    }
                    break;
                }
            case ESongStates.Finished:
                {
                    //after a while showing final score
                    currentMusicTime += Time.deltaTime;
                    if (timeStampToFinish < currentMusicTime)
                    {
                        songState = ESongStates.Finalized;
                        uILogicManager.UpdateFinalScore(GameMaster.Instance.musicLoader.DancerSongParsed.title, correctCount, totalMoveEvents, mistakeCount);
                        uILogicManager.ActivateEndingPanel();
                    }
                    break;
                }
            case ESongStates.Finalized:
                {
                    break;
                }
        }
    }

    IEnumerator CheckSongMovesLoaded()
    {
        int tries = 0;
        while(tries < Constants.MAX_LOAD_TRIES)
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

    void AddOneMistake()
    {
        mistakeCount++;
        uILogicManager.UpdateMissesUI(correctCount, mistakeCount, totalMoveEvents);
    }

    void AddOneCorrect()
    {
        correctCount++;
        uILogicManager.UpdateMissesUI(correctCount, mistakeCount, totalMoveEvents);
    }

    public void ReinitAndPlay()
    {
        moveSpeed = Constants.BASE_SPEED * speedDiffficulty;
        timeToReachChecker = Constants.DISTANCE_TO_CHECKER / moveSpeed;
        //TODO set another stage in loader
        //Set colors of move events
        moveFactory.SetMoveColor(0, GameMaster.Instance.musicLoader.DancerSongParsed.upArrowColor.ToUnityColor());
        moveFactory.SetMoveColor(1, GameMaster.Instance.musicLoader.DancerSongParsed.rightArrowColor.ToUnityColor());
        moveFactory.SetMoveColor(2, GameMaster.Instance.musicLoader.DancerSongParsed.leftArrowColor.ToUnityColor());
        moveFactory.SetMoveColor(3, GameMaster.Instance.musicLoader.DancerSongParsed.downArrowColor.ToUnityColor());

        //instantiate all move events
        tickPerSecond = GameMaster.Instance.musicLoader.DancerSongParsed.ticksPerSecond;
        moveEvents = moveFactory.GenerateGameMovesFromXml(GameMaster.Instance.musicLoader.DancerSongParsed.dancerEvents, tickPerSecond * speedDiffficulty);

        //instantiate all visual events
        visualEvents = effectsFactory.GenerateVisualEffectObjects(GameMaster.Instance.musicLoader.DancerSongParsed.dancerEvents, tickPerSecond * speedDiffficulty);
        //prepare mistake counts and update ui
        mistakeCount = 0;
        correctCount = 0;
        totalMoveEvents = GameMaster.Instance.musicLoader.DancerSongParsed.dancerEvents.movementEvents.Count;
        uILogicManager.UpdateMissesUI(correctCount, mistakeCount, totalMoveEvents);
        uILogicManager.UpdateTitle(GameMaster.Instance.musicLoader.DancerSongParsed.title);
        uILogicManager.SetMaxValue((moveEvents[moveEvents.Count - 1].GetComponent("IMoveEvent") as IMoveEvent).GetBeginTime() / tickPerSecond + timeToReachChecker);

        //set ui colors
        ArgbColor newColor = GameMaster.Instance.musicLoader.DancerSongParsed.upArrowColor;
        uiFxManager.SetArrowColor(MoveTypeEnum.Up, new Color((float)newColor.red / 255, (float)newColor.green / 255, (float)newColor.blue / 255, (float)newColor.alpha / 255));
        newColor = GameMaster.Instance.musicLoader.DancerSongParsed.rightArrowColor;
        uiFxManager.SetArrowColor(MoveTypeEnum.Right, new Color((float)newColor.red / 255, (float)newColor.green / 255, (float)newColor.blue / 255, (float)newColor.alpha / 255));
        newColor = GameMaster.Instance.musicLoader.DancerSongParsed.leftArrowColor;
        uiFxManager.SetArrowColor(MoveTypeEnum.Left, new Color((float)newColor.red / 255, (float)newColor.green / 255, (float)newColor.blue / 255, (float)newColor.alpha / 255));
        newColor = GameMaster.Instance.musicLoader.DancerSongParsed.downArrowColor;
        uiFxManager.SetArrowColor(MoveTypeEnum.Down, new Color((float)newColor.red / 255, (float)newColor.green / 255, (float)newColor.blue / 255, (float)newColor.alpha / 255));

        newColor = GameMaster.Instance.musicLoader.DancerSongParsed.backgroundColor;
        uiFxManager.SetBackgroundColor(new Color((float)newColor.red / 255, (float)newColor.green / 255, (float)newColor.blue / 255, (float)newColor.alpha / 255));
        newColor = GameMaster.Instance.musicLoader.DancerSongParsed.uiColor;
        uiFxManager.SetPanelUiColor(new Color((float)newColor.red / 255, (float)newColor.green / 255, (float)newColor.blue / 255, (float)newColor.alpha / 255));
        newColor = GameMaster.Instance.musicLoader.DancerSongParsed.uiTextColor;
        uiFxManager.SetTextColor(new Color((float)newColor.red / 255, (float)newColor.green / 255, (float)newColor.blue / 255, (float)newColor.alpha / 255));


        StartCoroutine(CheckSongMovesLoaded());
    }
}
