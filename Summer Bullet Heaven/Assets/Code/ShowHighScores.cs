using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHighScores : MonoBehaviour
{
    public TMPro.TextMeshProUGUI[] highScoreTexts;

    // Start is called before the first frame update
    void Start()
    {
        HighScoreTracker.LoadHighScores();
        for (int i = 0; i < highScoreTexts.Length; i++)
        {
            highScoreTexts[i].text = HighScoreTracker.highscores[i].ToString();
        }
    }
}
