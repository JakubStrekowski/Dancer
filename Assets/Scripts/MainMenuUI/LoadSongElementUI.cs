using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EPreviewLoadElementChildObjects
{
    previewImage = 0,
    titleText,
    desciptionText,
    playBtn
}

public class LoadSongElementUI : SongElementUI
{

    public override void OnInit(string path, float offset, Sprite image, string title, 
        string description, string score, Color scoreColor)
    {
        levelFolder = path;
        GetComponent<RectTransform>().position = new Vector3(
                GetComponent<RectTransform>().position.x,
                GetComponent<RectTransform>().position.y + offset, 
                GetComponent<RectTransform>().position.z);

        previewImage = transform.GetChild((int)EPreviewElementChildObjects.previewImage)
                            .GetComponent<Image>();
        previewImage.sprite = image;
        titleTxt = transform.GetChild((int)EPreviewElementChildObjects.titleText)
                            .GetComponent<TextMeshProUGUI>();
        titleTxt.text = title;
        descriptionTxt = transform.GetChild((int)EPreviewElementChildObjects.desciptionText)
                            .GetComponent<TextMeshProUGUI>();
        descriptionTxt.text = description;
        onPlayBtn = transform.GetChild((int)EPreviewElementChildObjects.playBtn)
                            .GetComponent<Button>();
        onPlayBtn.onClick.AddListener(OnPlayClicked);

        gameObject.SetActive(true);
    }


    public override void OnPlayClicked()
    {
        GameMaster.Instance.LoadSongToEditor(levelFolder);
    }
}
