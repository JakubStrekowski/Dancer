using MIDIparser.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadCustomSongs : MonoBehaviour
{
    private readonly float NEXT_ELEMENT_OFFSET = 100;
    private string MUSIC_PATH;

    private readonly string SCORE_MOCK = "0/100";

    [SerializeField]
    private MusicLoader loader;

    [SerializeField]
    private RectTransform scrollContent;

    [SerializeField]
    private GameObject previewSongObject;

    private List<DancerSong> dancerSongs;
    private float currentOffset;
    private Sprite previewImage;

    private ESongLoadStages currentLoadStage;

    public enum ESongLoadStages
    {
        loadingPreview,
        awaitingForNextDir
    }

    private void Awake()
    {
        dancerSongs = new List<DancerSong>();
        MUSIC_PATH = Application.dataPath + "/Resources/Music/";
    }

    private void Start()
    {
        loader = GameMaster.Instance.musicLoader;
    }

    public void OnCustomSongMenuOpened()
    {
        currentLoadStage = ESongLoadStages.loadingPreview;

        StartCoroutine(LoadAllLevels());
    }

    private IEnumerator LoadAllLevels()
    {

        for (int i = scrollContent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(scrollContent.transform.GetChild(i).gameObject);
        }

        List<string> directories = Directory.EnumerateDirectories(MUSIC_PATH).ToList();
        currentOffset = 0;
        int iter = 0;
        foreach (string dir in directories)
        {
            string fileName = dir.Split('/').Last();
            if (File.Exists(dir + '/' + fileName + ".xml"))
            {
                loader.LoadMusicMoves(fileName);
                dancerSongs.Add(loader.DancerSongParsed);

                GameObject newSongElement;

                DownloadImage(dir + '/' + dancerSongs[iter].imagePreviewPath);
                while(currentLoadStage == ESongLoadStages.loadingPreview)
                {
                    yield return new WaitForSeconds(0.2f);
                }

                newSongElement = GameObject.Instantiate(previewSongObject, scrollContent.transform);
                newSongElement.GetComponent<FoundSongElementUI>().OnInit(fileName, currentOffset, previewImage, dancerSongs[iter].title,
                   dancerSongs[iter].additionaldesc, SCORE_MOCK);

                currentOffset -= NEXT_ELEMENT_OFFSET;
                currentLoadStage = ESongLoadStages.loadingPreview;
                iter++;
            }
        }
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
                previewImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                currentLoadStage = ESongLoadStages.awaitingForNextDir;
            }
        }));
    }
}
