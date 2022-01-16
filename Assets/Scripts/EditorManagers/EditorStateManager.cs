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
    private EEditorStates editorState;

    private void OnEditorStateChanged()
    {

    }

    public void SetEditorState(int state)
    {
        editorState = (EEditorStates)state;
        OnEditorStateChanged();
    }
}
