using HSVPicker;
using MIDIparser.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
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

public enum EEditorTextInfoUIObjs
{
    title,
    description
}
public enum EEditorInfoColors
{
    None = -1,
    UpArrow,
    RightArrow,
    LeftArrow,
    DownArrow,
    UiBackground,
    UiText,
}



public class EditorUIManager : MonoBehaviour
{
    private EEditorToolState toolState;
    private EEditorToolEventMode toolEventMode;

    [SerializeField]
    private Button[] paletteButtons;

    #region SongInfoPanel
    //TODO move to another sctipt
    public EEditorInfoColors currentlyEditedColor = EEditorInfoColors.None;

    [SerializeField]
    private TextMeshProUGUI infoTexts;

    [SerializeField]
    private TMP_InputField[] infoInputFields;

    [SerializeField]
    private Image songImagePreview;

    [SerializeField]
    private ColorPicker colorPicker;

    [SerializeField]
    private Image[] colorsPreview;

    [SerializeField]
    private Image uiColorPreview;

    [SerializeField]
    private TextMeshProUGUI uiTextPreview;
    #endregion

    [SerializeField]
    private GameObject editStagePanel;

    private MusicLoader loader;
    private string MUSIC_PATH;


    private void Start()
    {
        loader = GameMaster.Instance.musicLoader;
        MUSIC_PATH = Application.dataPath + "/Resources/Music/";
    }

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

    public void RefreshEditorTexts()
    {
        DancerSong currentSong = GameMaster.Instance.musicLoader.DancerSongParsed;

        infoInputFields[(int)EEditorTextInfoUIObjs.title].text = currentSong.title;
        infoInputFields[(int)EEditorTextInfoUIObjs.description].text = currentSong.additionaldesc;
        infoTexts.text = MUSIC_PATH + '/' + currentSong.title + '/' + currentSong.musicFilePath;
        DownloadImage(MUSIC_PATH + '/' + currentSong.title + '/' + currentSong.imagePreviewPath);
    }

    public void RefreshEditorColors()
    {
        DancerSong currentSong = GameMaster.Instance.musicLoader.DancerSongParsed;

        colorsPreview[(int)EEditorInfoColors.UpArrow].color =
            currentSong.upArrowColor.ToUnityColor();
        colorsPreview[(int)EEditorInfoColors.RightArrow].color =
            currentSong.rightArrowColor.ToUnityColor();
        colorsPreview[(int)EEditorInfoColors.LeftArrow].color =
            currentSong.leftArrowColor.ToUnityColor();
        colorsPreview[(int)EEditorInfoColors.DownArrow].color =
            currentSong.downArrowColor.ToUnityColor();

        colorsPreview[(int)EEditorInfoColors.UiBackground].color =
            currentSong.uiColor.ToUnityColor();
        colorsPreview[(int)EEditorInfoColors.UiText].color =
            currentSong.uiTextColor.ToUnityColor();

        uiColorPreview.color = colorsPreview[(int)EEditorInfoColors.UiBackground].color;
        uiTextPreview.color = colorsPreview[(int)EEditorInfoColors.UiText].color;
    }

    public void SetCurrentlyEditedColor(int value)
    {
        currentlyEditedColor = (EEditorInfoColors)value;
        colorPicker.CurrentColor = colorsPreview[value].color;
    }

    public void OnColorChange()
    {
        DancerSong currentSong = GameMaster.Instance.musicLoader.DancerSongParsed;

        if (currentlyEditedColor == EEditorInfoColors.None) return;

        colorsPreview[(int)currentlyEditedColor].color = colorPicker.CurrentColor;

        if (currentlyEditedColor == EEditorInfoColors.UiBackground)
        {
            uiColorPreview.color = colorPicker.CurrentColor;
        }
        if (currentlyEditedColor == EEditorInfoColors.UiText)
        {
            uiTextPreview.color = colorPicker.CurrentColor;
        }

        switch (currentlyEditedColor)
        {
            case EEditorInfoColors.None:
                break;
            case EEditorInfoColors.UpArrow:
                currentSong.upArrowColor = 
                    new ArgbColor(colorsPreview[(int)currentlyEditedColor].color);
                break;
            case EEditorInfoColors.RightArrow:
                currentSong.rightArrowColor =
                    new ArgbColor(colorsPreview[(int)currentlyEditedColor].color);
                break;
            case EEditorInfoColors.LeftArrow:
                currentSong.leftArrowColor =
                    new ArgbColor(colorsPreview[(int)currentlyEditedColor].color);
                break;
            case EEditorInfoColors.DownArrow:
                currentSong.downArrowColor =
                    new ArgbColor(colorsPreview[(int)currentlyEditedColor].color);
                break;
            case EEditorInfoColors.UiBackground:
                currentSong.uiColor =
                    new ArgbColor(colorsPreview[(int)currentlyEditedColor].color);
                break;
            case EEditorInfoColors.UiText:
                currentSong.uiTextColor =
                    new ArgbColor(colorsPreview[(int)currentlyEditedColor].color);
                break;
            default:
                break;
        }
    }

    public void OnTitleChange()
    {
        DancerSong currentSong = GameMaster.Instance.musicLoader.DancerSongParsed;
        currentSong.title = 
            infoInputFields[(int)EEditorTextInfoUIObjs.title].text;
    }

    public void OnDescriptionChange()
    {
        DancerSong currentSong = GameMaster.Instance.musicLoader.DancerSongParsed;
        currentSong.additionaldesc = 
            infoInputFields[(int)EEditorTextInfoUIObjs.description].text;
    }

    public void SwitchStagePanel()
    {
        editStagePanel.SetActive(!editStagePanel.activeSelf);
    }

    public string GetMusicPath()
    {
        return infoTexts.text;
    }
    public Sprite GetPreviewPath()
    {
        return songImagePreview.sprite;
    }

    private void DownloadImage(string url)
    {
        StartCoroutine(loader.ImageRequest(url, (UnityWebRequest www) =>
        {
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log($"{www.error}: {www.downloadHandler.text}");
            }
            else
            {
                // Get the texture out using a helper downloadhandler
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                // Save it into the Image UI's sprite
                songImagePreview.sprite = Sprite.Create(
                    texture,
                    new Rect(
                        0,
                        0, 
                        texture.width, 
                        texture.height), 
                    new Vector2(0.5f, 0.5f));
            }
        }));
    }

}
