using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EPaletteItemChildObjects
{
    titleText = 0,
    previewImage,
}

public class PaletteItem : MonoBehaviour
{
    public TextMeshProUGUI previewItemName;
    public Image previewImage;
    protected Button onSelectedBtn;

    public void OnInit(string itemName, Sprite sprite, float offsetX, float offsetY)
    {
        GetComponent<RectTransform>().position = new Vector3(
            GetComponent<RectTransform>().position.x + offsetX,
            GetComponent<RectTransform>().position.y + offsetY,
            GetComponent<RectTransform>().position.z);

        previewImage = transform.GetChild((int)EPaletteItemChildObjects.previewImage)
            .GetComponent<Image>();
        previewImage.sprite = sprite;

        previewItemName = transform.GetChild((int)EPaletteItemChildObjects.titleText)
            .GetComponent<TextMeshProUGUI>();
        previewItemName.text = itemName;

        onSelectedBtn = GetComponent<Button>();
        onSelectedBtn.onClick.AddListener(OnItemSelected);

        gameObject.SetActive(true);

    }
    public void OnItemSelected()
    {
        //TODO select item from palette reaction
        ;
    }
}

