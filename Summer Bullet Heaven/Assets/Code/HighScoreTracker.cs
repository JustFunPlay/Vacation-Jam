using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public static class HighScoreTracker
{
    public static int[] highscores { get; private set; }

    public static void LoadHighScores()
    {
        highscores = new int[10];

        string[] filesStrings = Directory.GetFiles(Application.dataPath + "/StreamingAssets/XML/");
        foreach (string fileString in filesStrings)
        {
            if (fileString.Contains("HighScores.xml") && !fileString.Contains("xml.meta"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(int[]));
                StreamReader stream = new StreamReader(fileString);
                highscores = serializer.Deserialize(stream) as int[];
                stream.Close();
                //print(fileString);
            }
        }
    }

    public static void SaveHighScores()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(int[]));
        var encoding = System.Text.Encoding.GetEncoding("UTF-8");
        StreamWriter stream = new StreamWriter(Application.dataPath + "/StreamingAssets/XML/HighScores.xml", false, encoding);
        serializer.Serialize(stream, highscores);
        stream.Close();
    }

    public static void UpdateHighScores(int newHighScore)
    {
        int savedscore = 0;
        for (int i = 0; i < highscores.Length; i++)
        {
            if (newHighScore > highscores[i])
            {
                savedscore = highscores[i];
                highscores[i] = newHighScore;
                newHighScore = savedscore;
            }
        }
        SaveHighScores();
    }
}
