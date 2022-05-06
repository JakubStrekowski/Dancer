using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEditorStates
{
    basic,
    editMoves,
    editMovesPlay,
    editEvents,
    test
}

public class EditorStateManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    private EditorUIManager editorUIManager;
    [HideInInspector]
    public EEditorStates EditorState { get; private set; }

    private void Awake()
    {
        editorUIManager = GetComponent<EditorUIManager>();
    }

    public void OnLevelLoaded()
    {
        gameManager.CreateEvents();
    }

    public void ResetTestModeTime()
    {
        gameManager.SetSongProgress(0f);
    }

    public void SetEditorState(int state)
    {
        OnEditorStateLeave();
        EditorState = (EEditorStates)state;
        OnEditorStateEnter();
    }

    public void BeginTest()
    {
        switch (EditorState)
        {
            default:
            case EEditorStates.basic:
            case EEditorStates.test:
                SetEditorState((int)EEditorStates.test);
                break;
            case EEditorStates.editMoves:
            case EEditorStates.editMovesPlay:
            case EEditorStates.editEvents:
                SetEditorState((int)EEditorStates.editMovesPlay);
                break;
        }
    }
    public void StopTest()
    {
        switch (EditorState)
        {
            default:
            case EEditorStates.basic:
            case EEditorStates.test:
                SetEditorState((int)EEditorStates.basic);
                break;
            case EEditorStates.editMoves:
            case EEditorStates.editEvents:
            case EEditorStates.editMovesPlay:
                SetEditorState((int)EEditorStates.editMoves);
                break;
        }
    }

    private void OnEditorStateEnter()
    {
        switch (EditorState)
        {
            case EEditorStates.basic:
                break;
            case EEditorStates.editMoves:
                break;
            case EEditorStates.editMovesPlay:
                StartEditMovesPlay();
                break;
            case EEditorStates.editEvents:
                break;
            case EEditorStates.test:
                StartSongTest();
                break;
            default:
                break;
        }
    }

    private void OnEditorStateLeave()
    {
        switch (EditorState)
        {
            case EEditorStates.basic:
                break;
            case EEditorStates.editMoves:
                break;
            case EEditorStates.editMovesPlay:
                EndEditMovesPlay();
                break;
            case EEditorStates.editEvents:
                break;
            case EEditorStates.test:
                EndSongTest();
                break;
            default:
                break;
        }
    }

    private void StartSongTest()
    {
        gameManager.ResumeFromProgress();
        editorUIManager.OnTestModeStart();
    }

    private void EndSongTest()
    {
        gameManager.StopPlaying();
        editorUIManager.OnTestModeEnd();
    }

    private void StartEditMovesPlay()
    {
        gameManager.ResumeFromProgress();
        editorUIManager.OnCreateMovesPlayStart();
    }

    private void EndEditMovesPlay()
    {
        gameManager.StopPlaying();
        editorUIManager.OnCreateMovesPlayEnd();
    }
}
