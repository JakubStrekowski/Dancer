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

    [SerializeField]
    private MoveEventsBoard movesBoard;

    private AudioClip currentSong;
    private float timeToReachChecker = 5f;
    private float tickPerSecond = 1680f;

    List<GameObject> moveEvents;
    List<VisualEventBase> visualEvents;

    private float currentMusicTime = 0.0f;

    private float speedDiffficulty = 1.0f; //scaling BASE_SPEED to make lever harder
    public float moveSpeed = 10;

    //gameplay counters
    private int mistakeCount = 0;
    private int correctCount = 0;
    private int totalMoveEvents = 0;

    private int currentMoveId = 0;
    private int currentVisualFxId = 0;

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

    public void ResumeFromProgress()
    {
        songState = ESongStates.Loading;

        currentMusicTime = uILogicManager.songProgress.value;
        uILogicManager.UpdateProgress(currentMusicTime);
        Resume();
    }

    public void StopPlaying()
    {
        songState = ESongStates.AwaitingToStart;

        audioSrc.Stop();
        songWasPlayed = false;
    }
    public void CreateEvents()
    {
        //instantiate all move events
        movesBoard.ClearAllMoves();
        tickPerSecond = GameMaster.Instance.musicLoader.DancerSongParsed.ticksPerSecond;
        moveEvents = moveFactory.GenerateGameMovesFromXml(
            GameMaster.Instance.musicLoader.DancerSongParsed.dancerEvents,
            tickPerSecond * speedDiffficulty, currentMusicTime);

        //instantiate all visual events
        effectsFactory.DeleteAllVisualSprites();
        visualEvents = effectsFactory.GenerateVisualEffectObjects(
            GameMaster.Instance.musicLoader.DancerSongParsed.dancerEvents,
            tickPerSecond * speedDiffficulty);
    }

    public void RefreshGameUI()
    {
        //Set colors of move events
        moveFactory.SetMoveColor(MoveTypeEnum.Up,
            GameMaster.Instance.musicLoader.DancerSongParsed.upArrowColor.ToUnityColor());
        moveFactory.SetMoveColor(MoveTypeEnum.Right,
            GameMaster.Instance.musicLoader.DancerSongParsed.rightArrowColor.ToUnityColor());
        moveFactory.SetMoveColor(MoveTypeEnum.Left,
            GameMaster.Instance.musicLoader.DancerSongParsed.leftArrowColor.ToUnityColor());
        moveFactory.SetMoveColor(MoveTypeEnum.Down,
            GameMaster.Instance.musicLoader.DancerSongParsed.downArrowColor.ToUnityColor());

        //set ui colors
        ArgbColor newColor = GameMaster.Instance.musicLoader.DancerSongParsed.upArrowColor;
        uiFxManager.SetArrowColor(MoveTypeEnum.Up, ArgbColor.ConvertFromBytes(newColor));
        newColor = GameMaster.Instance.musicLoader.DancerSongParsed.rightArrowColor;
        uiFxManager.SetArrowColor(MoveTypeEnum.Right, ArgbColor.ConvertFromBytes(newColor));
        newColor = GameMaster.Instance.musicLoader.DancerSongParsed.leftArrowColor;
        uiFxManager.SetArrowColor(MoveTypeEnum.Left, ArgbColor.ConvertFromBytes(newColor));
        newColor = GameMaster.Instance.musicLoader.DancerSongParsed.downArrowColor;
        uiFxManager.SetArrowColor(MoveTypeEnum.Down, ArgbColor.ConvertFromBytes(newColor));

        newColor = GameMaster.Instance.musicLoader.DancerSongParsed.backgroundColor;
        uiFxManager.SetBackgroundColor(ArgbColor.ConvertFromBytes(newColor));
        newColor = GameMaster.Instance.musicLoader.DancerSongParsed.uiColor;
        uiFxManager.SetPanelUiColor(ArgbColor.ConvertFromBytes(newColor));
        newColor = GameMaster.Instance.musicLoader.DancerSongParsed.uiTextColor;
        uiFxManager.SetTextColor(ArgbColor.ConvertFromBytes(newColor));

        //prepare mistake counts and update ui
        mistakeCount = 0;
        correctCount = 0;
        totalMoveEvents =
            GameMaster.Instance.musicLoader.DancerSongParsed.dancerEvents.movementEvents.Count;
        uILogicManager.UpdateMissesUI(correctCount, mistakeCount, totalMoveEvents);
        uILogicManager.UpdateTitle(GameMaster.Instance.musicLoader.DancerSongParsed.title);
        uILogicManager.SetMaxValue((moveEvents[moveEvents.Count - 1]
            .GetComponent("IMoveEvent") as IMoveEvent)
            .GetBeginTime() / tickPerSecond + timeToReachChecker);

        RefreshMovesColors();
    }

    public void SetSongProgress(float time)
    {
        currentMusicTime = time;
        uILogicManager.UpdateProgress(currentMusicTime);

        mistakeCount = 0;
        correctCount = 0;
        uILogicManager.UpdateMissesUI(correctCount, mistakeCount, totalMoveEvents);

        if (songState == ESongStates.Playing)
        {
            songState = ESongStates.ReadyToPlay;
            ResetMoveStates();
            audioSrc.Stop();
        }
        else
        {
            songState = ESongStates.AwaitingToStart;
        }
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
                    audioSrc.time = currentMusicTime > timeToReachChecker ?
                        currentMusicTime - timeToReachChecker : 0.0f;
                    CalculateCurrentMoveId();
                    songState = ESongStates.Playing;
                    return;
                }
            case ESongStates.Playing:
                {
                    currentMusicTime += Time.deltaTime;

                    if (currentMusicTime > timeToReachChecker && 
                        audioSrc.isPlaying == false)
                    {
                        if (songWasPlayed &&
                            currentMusicTime > audioSrc.clip.length) //finishing song condiion
                        {
                            songState = ESongStates.Finished;
                            timeStampToFinish = 
                                currentMusicTime + Constants.TIME_AFTER_SONG_AWAIT;
                            break;
                        }

                        audioSrc.Play();
                        songWasPlayed = true;
                    }

                    uILogicManager.UpdateProgress(currentMusicTime);

                    //setting move events active when they reach (time - time for them to reach note checker)
                    if (moveEvents.Count != 0)
                    {
                        for (; currentMoveId < moveEvents.Count; currentMoveId++)
                        {
                            if (((moveEvents[currentMoveId].GetComponent("IMoveEvent") as IMoveEvent)
                            .GetBeginTime() / tickPerSecond) < currentMusicTime)
                            {
                                (moveEvents[currentMoveId].GetComponent("IMoveEvent") as IMoveEvent)
                                    .SetActivateEvent(true);
                            }
                            else break;
                        }
                    }

                    //setting visual events active when they reach their time
                    if (visualEvents.Count != 0)
                    {
                        while((visualEvents[0].startTime / tickPerSecond + timeToReachChecker) 
                            < currentMusicTime)
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
                        uILogicManager.UpdateFinalScore(
                            GameMaster.Instance.musicLoader.DancerSongParsed.title,
                            correctCount, totalMoveEvents, mistakeCount);
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

    private void ReinitAndPlay()
    {

        moveSpeed = Constants.BASE_SPEED * speedDiffficulty;
        timeToReachChecker = Constants.DISTANCE_TO_CHECKER / moveSpeed;

        RefreshGameUI();
        CreateEvents();

        currentMoveId = 0;

        StartCoroutine(CheckSongMovesLoaded());
    }
    private void Resume()
    {

        moveSpeed = Constants.BASE_SPEED * speedDiffficulty;
        timeToReachChecker = Constants.DISTANCE_TO_CHECKER / moveSpeed;

        RefreshGameUI(); 

        StartCoroutine(CheckSongMovesLoaded());
    }

    private void ResetMoveStates()
    {
        for (int i = 0; i < moveEvents.Count; i++)
        {
            (moveEvents[i].GetComponent("IMoveEvent") as IMoveEvent).
                SetActivateEvent(false);
        }
    }

    private void CalculateCurrentMoveId()
    {
        if (moveEvents.Count != 0)
        {
            for (currentMoveId = 0; currentMoveId < moveEvents.Count; currentMoveId++)
            {
                if (((moveEvents[currentMoveId].GetComponent("IMoveEvent") as IMoveEvent)
                .GetBeginTime() / tickPerSecond) >= currentMusicTime)
                {
                    break;
                }
            }
        }
    }

    private void RefreshMovesColors()
    {
        if (moveEvents != null)
        {
            foreach (var moveEvent in moveEvents)
            {
                switch (moveEvent.GetComponent<IMoveEvent>().GetEventTypeID())
                {
                    case MoveTypeEnum.Up:
                        moveEvent.GetComponent<IMoveEvent>().SetColor(
                            GameMaster.Instance.musicLoader.DancerSongParsed.
                                upArrowColor.ToUnityColor());
                        break;
                    case MoveTypeEnum.Right:
                        moveEvent.GetComponent<IMoveEvent>().SetColor(
                            GameMaster.Instance.musicLoader.DancerSongParsed.
                                rightArrowColor.ToUnityColor());
                        break;
                    case MoveTypeEnum.Left:
                        moveEvent.GetComponent<IMoveEvent>().SetColor(
                            GameMaster.Instance.musicLoader.DancerSongParsed.
                                leftArrowColor.ToUnityColor());
                        break;
                    case MoveTypeEnum.Down:
                        moveEvent.GetComponent<IMoveEvent>().SetColor(
                            GameMaster.Instance.musicLoader.DancerSongParsed.
                                downArrowColor.ToUnityColor());
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
