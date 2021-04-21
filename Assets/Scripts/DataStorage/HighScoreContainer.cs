using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HighScoreContainer
{
    public List<HighScore> scores;

    public HighScoreContainer()
    {
        scores = new List<HighScore>();
    }
}
