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

    //void OnEnable()
    //{
    //    currentMusicTime = 0.0f;
    //    ReinitAndPlay();
    //}

    public void ResumeFromProgress()
    {
        songState = ESongStates.Loading;

        currentMusicTime = uILogicManager.songProgress.value;
        ReinitAndPlay(currentMusicTime);
    }

    public void StopPlaying()
    {
        effectsFactory.DeleteAllVisualSprites();

        songState = ESongStates.AwaitingToStart;

        audioSrc.Stop();
        songWasPlayed = false;
    }

    //private void OnDisable()
    //{
    //    foreach(GameObject moveEventObject in moveEvents)
    //    {
    //        Destroy(moveEventObject);
    //    }
    //
    //    effectsFactory.DeleteAllVisualSprites();
    //
    //    songState = ESongStates.AwaitingToStart;
    //
    //    audioSrc.Stop();
    //    songWasPlayed = false;
    //}

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
                        while(((moveEvents[0].GetComponent("IMoveEvent") as IMoveEvent)
                            .GetBeginTime() / tickPerSecond) < currentMusicTime)
                        {
                            (moveEvents[0].GetComponent("IMoveEvent") as IMoveEvent)
                                .ActivateEvent(moveSpeed);
                            moveEvents.RemoveAt(0);
                            if (moveEvents.Count == 0) break;
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

    public void ReinitAndPlay(float fromTime = 0.0f)
    {

        moveSpeed = Constants.BASE_SPEED * speedDiffficulty;
        timeToReachChecker = Constants.DISTANCE_TO_CHECKER / moveSpeed;
        //TODO set another stage in loader
        //Set colors of move events
        moveFactory.SetMoveColor(MoveTypeEnum.Up, 
            GameMaster.Instance.musicLoader.DancerSongParsed.upArrowColor.ToUnityColor());
        moveFactory.SetMoveColor(MoveTypeEnum.Right, 
            GameMaster.Instance.musicLoader.DancerSongParsed.rightArrowColor.ToUnityColor());
        moveFactory.SetMoveColor(MoveTypeEnum.Left, 
            GameMaster.Instance.musicLoader.DancerSongParsed.leftArrowColor.ToUnityColor());
        moveFactory.SetMoveColor(MoveTypeEnum.Down, 
            GameMaster.Instance.musicLoader.DancerSongParsed.downArrowColor.ToUnityColor());

        //instantiate all move events
        if (fromTime == 0)
        {
            movesBoard.ClearAllMoves();
            tickPerSecond = GameMaster.Instance.musicLoader.DancerSongParsed.ticksPerSecond;
            moveEvents = moveFactory.GenerateGameMovesFromXml(
                GameMaster.Instance.musicLoader.DancerSongParsed.dancerEvents,
                tickPerSecond * speedDiffficulty, currentMusicTime);
        }
        //instantiate all visual events
        visualEvents = effectsFactory.GenerateVisualEffectObjects(
            GameMaster.Instance.musicLoader.DancerSongParsed.dancerEvents,
            tickPerSecond * speedDiffficulty);
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


        StartCoroutine(CheckSongMovesLoaded());
    }
}
