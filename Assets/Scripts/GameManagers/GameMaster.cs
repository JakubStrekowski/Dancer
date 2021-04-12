using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public MusicLoader musicLoader;
    public AudioClip nextMusic;
    private static GameMaster _instance;

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
        }
        DontDestroyOnLoad(this.gameObject);

        musicLoader = new MusicLoader();
        //TODO set another stage in loader
        musicLoader.LoadMusicMoves();
        GetAudioClip();
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    public void GetAudioClip()
    {
        StartCoroutine(musicLoader.GetAudioClipFromFile("Test", Callback));
    }

    public void Callback(AudioClip ac)
    {
        nextMusic = ac;
    }
}
