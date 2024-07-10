using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatDirector : MonoBehaviour
{
    public static CombatDirector instance { get; private set; }

    [SerializeField] private int difficultyLevel;
    [SerializeField] private int spawnPoints;
    [SerializeField] private SpawnableEnemy[] spawnableEnemies;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMPro.TextMeshProUGUI gameOverText;
    [SerializeField] private TMPro.TextMeshProUGUI HighScoreText;


    private int scoreEarned;
    private int timePlayed;
    private bool gameOver;

    private void Start()
    {
        instance = this;
        difficultyLevel = 0;
        spawnPoints = Random.Range(10, 20);
        TrySpawnEnemies();
        StartCoroutine(Director());
    }
    
    private IEnumerator Director()
    {
        yield return null;
        timePlayed = 0;
        while (!gameOver)
        {
            yield return new WaitForSeconds(1);
            timePlayed++;
            AddHighScore(10);
            spawnPoints += timePlayed % 2;
            if (timePlayed % 3 == Random.Range(0, 3))
                spawnPoints += 2 + difficultyLevel;
            if (timePlayed % 5 == 0)
                TrySpawnEnemies();
            if (timePlayed % 60 == 0)
            {
                difficultyLevel++;
                AddHighScore(difficultyLevel * 100);
            }
            timeText.text = $"{difficultyLevel}:{timePlayed % 60}";
        }
    }

    public void AddPoints(int points)
    {
        spawnPoints += points;
    }
    private void TrySpawnEnemies()
    {
        List<SpawnableEnemy> availableSpawns = new List<SpawnableEnemy>();
        for (int i = 0; i < spawnableEnemies.Length; i++)
        {
            if (spawnableEnemies[i].minimumDifficulty <= difficultyLevel)
                availableSpawns.Add(spawnableEnemies[i]);
        }

        if (availableSpawns.Count == 0)
            return;

        bool keepSpawning = true;
        while (keepSpawning && spawnPoints > 0)
        {
            int rSpawn = Random.Range(0, availableSpawns.Count);
            if (availableSpawns[rSpawn].spawnCost <= spawnPoints)
            {
                spawnPoints -= availableSpawns[rSpawn].spawnCost;
                Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                EnemyBase newEnemy = Instantiate(availableSpawns[rSpawn].enemy, PlayerControl.CurrentPlayer.transform.position + direction * Random.Range(25f, 40f), Quaternion.identity);
                newEnemy.ReadyUp(difficultyLevel);
            }
            else
                keepSpawning = false;
        }
    }

    public void AddHighScore(int score)
    {
        if (gameOver) return;
        scoreEarned += score;
        HighScoreText.text = $"Score: {scoreEarned}";
    }
    public void GameOver()
    {
        if (gameOver) return;
        gameOver = true;
        gameOverScreen.SetActive(true);
        gameOverText.text = $"You survived for {difficultyLevel} minutes and {timePlayed % 60} seconds\nYou have earned a total of {scoreEarned} points";
        HighScoreTracker.UpdateHighScores(scoreEarned);
    }
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}

[System.Serializable]
public class SpawnableEnemy
{
    public EnemyBase enemy;
    public int spawnCost;
    public int minimumDifficulty;
}