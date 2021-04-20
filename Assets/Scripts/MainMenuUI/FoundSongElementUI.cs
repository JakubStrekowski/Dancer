using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EPreviewElementChildObjects
{
    previewImage = 0,
    titleText,
    desciptionText,
    scoreText,
    playBtn
}

public class FoundSongElementUI : MonoBehaviour
{
    public string levelPath;

    Image previewImage;
    TextMeshProUGUI titleTxt;
    TextMeshProUGUI descriptionTxt;
    TextMeshProUGUI scoreTxt;

    Button onPlayBtn;

    public void OnInit(string path, float offset, Sprite image, string title, string description, string score)
    {
        levelPath = path;
        GetComponent<RectTransform>().position = new Vector3(GetComponent<RectTransform>().position.x,
            GetComponent<RectTransform>().position.y + offset, GetComponent<RectTransform>().position.z);

        previewImage = transform.GetChild((int)EPreviewElementChildObjects.previewImage).GetComponent<Image>();
        previewImage.sprite = image;
        titleTxt = transform.GetChild((int)EPreviewElementChildObjects.titleText).GetComponent<TextMeshProUGUI>();
        titleTxt.text = title;
        descriptionTxt = transform.GetChild((int)EPreviewElementChildObjects.desciptionText).GetComponent<TextMeshProUGUI>();
        descriptionTxt.text = description;
        scoreTxt = transform.GetChild((int)EPreviewElementChildObjects.scoreText).GetComponent<TextMeshProUGUI>();
        scoreTxt.text = score;
        onPlayBtn = transform.GetChild((int)EPreviewElementChildObjects.playBtn).GetComponent<Button>();

        gameObject.SetActive(true);
    }
}
