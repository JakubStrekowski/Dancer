using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HighScore
{
    public string levelName;
    public int score;
    public int misses;
    public int grade;

    public HighScore()
    {

    }

    public HighScore(string levelName, int score, int misses, int grade)
    {
        this.levelName = levelName;
        this.score = score;
        this.misses = misses;
        this.grade = grade;
    }
}
