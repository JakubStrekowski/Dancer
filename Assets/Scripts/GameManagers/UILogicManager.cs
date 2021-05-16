using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ELevelCompleteGrades
{
    platinum = 0,
    gold,
    silver,
    bronze
}


public class UILogicManager : MonoBehaviour
{
    public static readonly Color[] FINAL_TEXT_COLORS = { new Color(0.9f, 0.9f, 0.9f), new Color(1, 1, 0.6f), new Color(0.6f, 0.6f, 0.6f), new Color(0.5f, 0.4f, 0f) };

    public TextMeshProUGUI missesTxt;
    public GameObject gameEndedPanel;
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI newHighScore;
    public Slider songProgress;
    public TextMeshProUGUI titleTxt;

    public void UpdateTitle(string value)
    {
        titleTxt.text = value;
    }

    public void SetMaxValue(float value)
    {
        songProgress.maxValue = value;
    }

    public void UpdateProgress(float value)
    {
        songProgress.value = value;
    }

    public void UpdateMissesUI(int correctCount, int currentMisses, int totalMoves)
    {
        missesTxt.text = "Hits: " + correctCount + '/' + totalMoves + " Misses: " + currentMisses;
    }

    public void ActivateEndingPanel()
    {
        gameEndedPanel.gameObject.SetActive(true);
    }

    public void UpdateFinalScore(string levelName, int currentPasses, int totalMoves, int currentMisses)
    {
        finalScore.text = "Correct: " + currentPasses + "/" + totalMoves + " Misses:" + currentMisses;
        int grade;
        float scorepercentage = (float)currentPasses / (float)totalMoves;
        if(scorepercentage == 1.0f && currentMisses == 0)
        {
            finalScore.color = FINAL_TEXT_COLORS[(int)ELevelCompleteGrades.platinum];
            grade = (int)ELevelCompleteGrades.platinum;
        }
        else if (scorepercentage >= 0.8f && currentMisses <= (int)(totalMoves * 0.15f))
        {
            finalScore.color = FINAL_TEXT_COLORS[(int)ELevelCompleteGrades.gold];
            grade = (int)ELevelCompleteGrades.gold;
        }
        else if (scorepercentage >= 0.6f && currentMisses <= (int)(totalMoves * 0.3f))
        {
            finalScore.color = FINAL_TEXT_COLORS[(int)ELevelCompleteGrades.silver];
            grade = (int)ELevelCompleteGrades.silver;
        }
        else
        {
            finalScore.color = FINAL_TEXT_COLORS[(int)ELevelCompleteGrades.bronze];
            grade = (int)ELevelCompleteGrades.bronze;
        }

        if (DataManager.Instance.isNewHighScore(levelName, currentPasses, currentMisses, grade))
        {
            ActivateNewHighScorePopup();
            DataManager.Instance.AddNewScore(levelName, currentPasses, currentMisses, grade);
            DataManager.Instance.SaveHighScores();
        }
    }

    public void ActivateNewHighScorePopup()
    {
        newHighScore.gameObject.SetActive(true);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene((int)ESceneIndexes.mainMenuSceneIndex);
    }

}
