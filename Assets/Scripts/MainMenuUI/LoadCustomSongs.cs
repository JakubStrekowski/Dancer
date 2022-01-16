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
    private string PREDEFINED_MUSIC_PATH;

    private readonly string SCORE_MOCK = "No highscore yet";

    [SerializeField]
    private MusicLoader loader;

    [SerializeField]
    private RectTransform scrollContent;

    [SerializeField]
    private RectTransform predefinedScrollContent;

    [SerializeField]
    private GameObject previewSongObject;

    private List<DancerSong> dancerSongs;
    private float currentOffset;
    private Sprite previewImage;
    private RectTransform selectedContent;

    private ESongLoadStages currentLoadStage;

    public enum ESongLoadStages
    {
        readyToOperate = 0,
        loadingPreview,
        awaitingForNextDir,
    }

    private void Awake()
    {
        dancerSongs = new List<DancerSong>();
        MUSIC_PATH = Application.dataPath + "/Resources/Music/";
        PREDEFINED_MUSIC_PATH = Application.dataPath + "/Resources/PredefinedMusic/";
    }

    private void Start()
    {
        loader = GameMaster.Instance.musicLoader;
    }
    public void OnPredefinedSongMenuOpened()
    {
        if(currentLoadStage == ESongLoadStages.readyToOperate)
        {
            currentLoadStage = ESongLoadStages.loadingPreview;
            loader.musicPath = PREDEFINED_MUSIC_PATH;
            selectedContent = predefinedScrollContent;
            dancerSongs = new List<DancerSong>();

            StartCoroutine(LoadAllLevels());
        }
    }

    public void OnCustomSongMenuOpened()
    {
        if (currentLoadStage == ESongLoadStages.readyToOperate)
        {
            currentLoadStage = ESongLoadStages.loadingPreview;
            loader.musicPath = MUSIC_PATH;
            selectedContent = scrollContent;
            dancerSongs = new List<DancerSong>();

            StartCoroutine(LoadAllLevels());
        }
    }

    private IEnumerator LoadAllLevels()
    {

        for (int i = selectedContent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(selectedContent.transform.GetChild(i).gameObject);
        }

        List<string> directories = Directory.EnumerateDirectories(loader.musicPath).ToList();
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

                HighScore highScore = DataManager.Instance.GetScoreByLevel(loader.DancerSongParsed.title);

                string highScoreText = "";
                Color scoreColor;
                if (highScore is null) 
                {
                    scoreColor = Color.white;
                    highScoreText = SCORE_MOCK;
                }
                else
                {
                    scoreColor = UILogicManager.FINAL_TEXT_COLORS[highScore.grade];
                    highScoreText = "HighScore: hit " + highScore.score.ToString() + " / miss " + highScore.misses.ToString();
                }


                newSongElement = GameObject.Instantiate(previewSongObject, selectedContent.transform);
                newSongElement.GetComponent<SongElementUI>().OnInit(fileName, currentOffset, previewImage, dancerSongs[iter].title,
                   dancerSongs[iter].additionaldesc, highScoreText, scoreColor);

                currentOffset -= NEXT_ELEMENT_OFFSET;
                currentLoadStage = ESongLoadStages.loadingPreview;
                iter++;
            }
        }
        currentLoadStage = ESongLoadStages.readyToOperate;
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

    public void ExitGame()
    {
        Application.Quit();
    }
}
