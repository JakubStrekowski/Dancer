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



public class EditorUIManager : MonoBehaviour
{
    private EEditorToolState toolState;
    private EEditorToolEventMode toolEventMode;

    [SerializeField]
    private Button[] paletteButtons;

    [SerializeField]
    private TextMeshProUGUI infoTexts;

    [SerializeField]
    private TMP_InputField[] infoInputFields;

    [SerializeField]
    private Image songImagePreview;


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

    public void RefreshEditorTexts(DancerSong dancerSong)
    {
        infoInputFields[(int)EEditorTextInfoUIObjs.title].text = dancerSong.title;
        infoInputFields[(int)EEditorTextInfoUIObjs.description].text = dancerSong.additionaldesc;
        infoTexts.text = dancerSong.musicFilePath;
        DownloadImage(MUSIC_PATH + '/' + dancerSong.title + '/' + dancerSong.imagePreviewPath);
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
