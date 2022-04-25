using MIDIparser.Models;
using MIDIparser.Models.VisualEventsSubclasses;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using SimpleFileBrowser;
using System;
using System.IO;

public class PaletteManager : MonoBehaviour
{
    private readonly int VERTICAL_OFFSET = -150;
    private readonly int HORIZONTAL_OFFSET = 140;

    private List<PaletteItem> paletteItems;
    Dictionary<string, Sprite> spriteCollection;

    [SerializeField]
    private RectTransform scrollContent;

    [SerializeField]
    private GameObject palettePrefab;

    void Awake()
    {
        paletteItems = new List<PaletteItem>();
        spriteCollection = new Dictionary<string, Sprite>();
    }

    public void PopulateFromLoadedSong(DancerSong song)
    {
        foreach(PaletteItem paletteItem in paletteItems)
        {
            for (int i = paletteItem.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(paletteItem.transform.GetChild(i).gameObject);
            }
        }

        spriteCollection.Clear();
        paletteItems.Clear();

        List<VisualEventBase> spriteEvents =
            song.dancerEvents.visualEvents.Where(x => 
                x.eventType == VisualEventTypeEnum.CreateObject ||
                x.eventType == VisualEventTypeEnum.ChangeSprite).ToList();

        foreach (VisualEventBase visualEvent in spriteEvents)
        {
            DownloadImageToCollection(GameMaster.Instance.musicLoader.musicPath +
                GameMaster.Instance.musicLoader.DancerSongParsed.title + "/" +
                visualEvent.paramsList[(int)CreateParamsEnum.spritePath], 
                visualEvent.paramsList[(int)CreateParamsEnum.spritePath]);
        }
    }

    public void OnLoadImageClick()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Images", ".png"));
        FileBrowser.SetDefaultFilter(".png");

        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true,
            null, null, "Add new image to palette", "Add");

        if (FileBrowser.Success)
        {
            foreach (string path in FileBrowser.Result)
            {
                string name = path.Split('\\', '/').Last();
                DownloadImageToCollection(path, name);
            }
        }
    }

    private void DownloadImageToCollection(string url, string name)
    {
        StartCoroutine(GameMaster.Instance.musicLoader.ImageRequest(url, 
            (UnityWebRequest www) =>
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
                if (!spriteCollection.ContainsKey(name))
                {
                    Sprite newSprite = Sprite.Create(
                        texture, new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f));

                    AddNewItemPalette(name, newSprite);
                }
            }
        }));
    }

    private void AddNewItemPalette(string name, Sprite newSprite)
    {
        spriteCollection[name] = newSprite;

        GameObject newPaletteElement = Instantiate(palettePrefab,
            scrollContent.transform);

        newPaletteElement.GetComponent<PaletteItem>().OnInit(
            name, newSprite,
            (paletteItems.Count % 2) * HORIZONTAL_OFFSET,
            paletteItems.Count / 2 * VERTICAL_OFFSET);

        paletteItems.Add(newPaletteElement.GetComponent<PaletteItem>());

        scrollContent.sizeDelta =
            new Vector2(0, paletteItems.Count / 2 * Mathf.Abs(VERTICAL_OFFSET));
    }
} 
