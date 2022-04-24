using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public abstract class SongElementUI : MonoBehaviour
{
    public string levelFolder;

    protected Image previewImage;
    protected TextMeshProUGUI titleTxt;
    protected TextMeshProUGUI descriptionTxt;
    protected TextMeshProUGUI scoreTxt;

    protected Button onPlayBtn;

    public abstract void OnInit(
        string path, float offset, Sprite image, 
        string title, string description, 
        string score, Color scoreColor);

    public abstract void OnPlayClicked();
}
