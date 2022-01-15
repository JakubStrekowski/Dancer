using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EEditorToolState
{
    none,
    create,
    select,
    move,
    rotate,
    scale,
    recolor,
    delete
}

public enum EEditorButton
{
    create,
    select,
    move,
    rotate,
    scale,
    recolor,
    linArc,
    delete
}

public enum EEditorToolEventMode
{
    linear,
    arc,
}


public class EditorUIManager : MonoBehaviour
{
    private EEditorToolState toolState;
    private EEditorToolEventMode toolEventMode;

    [SerializeField]
    private Button[] paletteButtons;

    private void RefreshButtonStates()
    {
        foreach (Button b in paletteButtons)
        {
            b.interactable = true;
        }
        switch (toolState)
        {
            case EEditorToolState.none:
                break;
            case EEditorToolState.create:
                paletteButtons[(int)EEditorButton.create].interactable = false;
                break;
            case EEditorToolState.select:
                paletteButtons[(int)EEditorButton.select].interactable = false;
                break;
            case EEditorToolState.move:
                paletteButtons[(int)EEditorButton.move].interactable = false;
                break;
            case EEditorToolState.rotate:
                paletteButtons[(int)EEditorButton.rotate].interactable = false;
                break;
            case EEditorToolState.scale:
                paletteButtons[(int)EEditorButton.scale].interactable = false;
                break;
            case EEditorToolState.recolor:
                paletteButtons[(int)EEditorButton.recolor].interactable = false;
                break;
            case EEditorToolState.delete:
                paletteButtons[(int)EEditorButton.delete].interactable = false;
                break;
        }
    }

    public void SetToolState(int newState)
    {
        toolState = (EEditorToolState)newState;
        RefreshButtonStates();
    }

    public void SwitchToolEventMode()
    {
        if (toolEventMode == EEditorToolEventMode.linear)
        {
            toolEventMode = EEditorToolEventMode.arc;
            paletteButtons[(int)EEditorButton.linArc].GetComponentInChildren<Text>().text = "Li";
        }
        else if (toolEventMode == EEditorToolEventMode.arc)
        {
            toolEventMode = EEditorToolEventMode.linear;
            paletteButtons[(int)EEditorButton.linArc].GetComponentInChildren<Text>().text = "Ar";
        }
    }
}
