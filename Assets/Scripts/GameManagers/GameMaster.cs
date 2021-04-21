using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private readonly int PLAY_SONG_SCENE_INDEX = 1;

    public MusicLoader musicLoader;
    public AudioClip nextMusic;
    private static GameMaster _instance;
    private string selectedSongFile;

    public static GameMaster Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;

            if (SceneManager.GetActiveScene().buildIndex == PLAY_SONG_SCENE_INDEX) //loaded level without main menu
            {
                selectedSongFile = "Test";
                musicLoader = new MusicLoader();
                musicLoader.LoadMusicMoves();
                GetAudioClip();
                return;
            }
        }
        DontDestroyOnLoad(this.gameObject);

        musicLoader = new MusicLoader();
        //TODO set another stage in loader
    }

    public void BeginSongLevel()
    {
        SceneManager.LoadScene(PLAY_SONG_SCENE_INDEX);
        musicLoader.LoadMusicMoves(selectedSongFile);
        GetAudioClip();
    }

    public void GetAudioClip()
    {
        StartCoroutine(musicLoader.GetAudioClipFromFile(selectedSongFile, Callback));
    }

    public void Callback(AudioClip ac)
    {
        nextMusic = ac;
    }

    public void SetSelectedSong(string song)
    {
        selectedSongFile = song;
    }
}
