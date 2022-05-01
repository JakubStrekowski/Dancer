using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ESceneIndexes
{
    mainMenuSceneIndex = 0,
    playSongSceneIndex = 1,
    editorSceneIndex = 2,
}

public class GameMaster : MonoBehaviour
{

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

            if (SceneManager.GetActiveScene().buildIndex == 
                (int)ESceneIndexes.playSongSceneIndex) //loaded level without main menu
            {
                selectedSongFile = "AVGVSTA - Together Again";
                musicLoader = new MusicLoader
                {
                    musicPath = Application.dataPath + "/Resources/PredefinedMusic/"
                };
                musicLoader.LoadMusicMoves(selectedSongFile);
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
        nextMusic = null;
        SceneManager.LoadScene((int)ESceneIndexes.playSongSceneIndex);
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

    public void LoadSongToEditor(string song)
    {
        selectedSongFile = song;
        musicLoader.LoadMusicMoves(selectedSongFile);
        GetAudioClip();
        GameObject.Find("EditorManager").GetComponent<EditorStateManager>().
            OnLevelLoaded();
        GameObject.Find("EditorManager").GetComponent<EditorUIManager>().
            RefreshEditorTexts(); 
        GameObject.Find("EditorManager").GetComponent<EditorUIManager>().
            RefreshEditorColors();
        GameObject.Find("EditorManager").GetComponent<PaletteManager>().
            PopulateFromLoadedSong(musicLoader.DancerSongParsed);
        GameObject.Find("CustomGamesPanel").SetActive(false);
    }

    public string GetEditorMusicPath()
    {
        return GameObject.Find("EditorManager").
            GetComponent<EditorUIManager>().GetMusicPath();
    }
    public Sprite GetEditorImagePreview()
    { 
        return GameObject.Find("EditorManager").
            GetComponent<EditorUIManager>().GetPreviewPath();
    }

    public Sprite GetSpriteFromPalette(string imageName)
    {
        return GameObject.Find("EditorManager").
            GetComponent<PaletteManager>().GetByName(imageName);
    }
}
