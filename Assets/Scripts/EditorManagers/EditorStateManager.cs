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
    private EEditorStates editorState;

    private void Awake()
    {
        editorUIManager = GetComponent<EditorUIManager>();
    }

    public void SetEditorState(int state)
    {
        OnEditorStateLeave();
        editorState = (EEditorStates)state;
        OnEditorStateEnter();
    }

    private void OnEditorStateEnter()
    {
        switch (editorState)
        {
            case EEditorStates.basic:
                break;
            case EEditorStates.editMoves:
                break;
            case EEditorStates.editMovesPlay:
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
        switch (editorState)
        {
            case EEditorStates.basic:
                break;
            case EEditorStates.editMoves:
                break;
            case EEditorStates.editMovesPlay:
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
}
