using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private readonly string HIGHSCORE_FOLDER = "Saved";
    private readonly string HIGHSCORE_FILE_NAME = "scores.json";

    private HighScoreContainer highScores;
    private static DataManager _instance;

    public static DataManager Instance { get { return _instance; } }
    // Start is called before the first frame update
    void Awake()
    {
        
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;

            if (!Directory.Exists(Application.dataPath + "/" + HIGHSCORE_FOLDER))
            {
                Directory.CreateDirectory(Application.dataPath + "/" + HIGHSCORE_FOLDER);
            }

            highScores = new HighScoreContainer();

            if(File.Exists(Application.dataPath + "/" + HIGHSCORE_FOLDER + "/" + HIGHSCORE_FILE_NAME))
            {
                var inputString = File.ReadAllText(Application.dataPath + "/" + HIGHSCORE_FOLDER + "/" + HIGHSCORE_FILE_NAME);
                highScores = JsonUtility.FromJson<HighScoreContainer>(inputString);
            }

        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SaveHighScores()
    {
        File.Delete(Application.dataPath + "/" + HIGHSCORE_FOLDER + "/" + HIGHSCORE_FILE_NAME);
        var outputString = JsonUtility.ToJson(highScores);
        File.WriteAllText(Application.dataPath + "/" + HIGHSCORE_FOLDER + "/" + HIGHSCORE_FILE_NAME, outputString);
    }

    public bool isNewHighScore(string levelName, int score,int misses, int grade)
    {
        HighScore prevScore = highScores.scores.Find(x => x.levelName == levelName);

        if (prevScore is null) return true;

        if (prevScore.grade > grade) return true;

        if (prevScore.grade == grade && prevScore.score < score) return true;

        if (prevScore.grade == grade && prevScore.score == score && prevScore.misses > misses) return true;

        return false;
    }

    public void AddNewScore(string levelName, int score, int misses, int grade)
    {
        HighScore prevScore = highScores.scores.Find(x => x.levelName == levelName);
        if(prevScore != null)
        {
            highScores.scores.Remove(prevScore);
        }
        highScores.scores.Add(new HighScore(levelName, score, misses, grade));
    }

    public HighScore GetScoreByLevel(string levelName)
    {
        return highScores.scores.Find(x => x.levelName == levelName);
    }
}
