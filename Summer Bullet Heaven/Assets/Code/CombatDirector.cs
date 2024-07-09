using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDirector : MonoBehaviour
{
    public static CombatDirector instance { get; private set; }

    [SerializeField] private int difficultyLevel;
    [SerializeField] private int spawnPoints;
    [SerializeField] private SpawnableEnemy[] spawnableEnemies;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;

    private void Start()
    {
        instance = this;
        difficultyLevel = 1;
        spawnPoints = Random.Range(10, 20);
        TrySpawnEnemies();
        StartCoroutine(Director());
    }
    
    private IEnumerator Director()
    {
        yield return null;
        int t = 0;
        while (true)
        {
            yield return new WaitForSeconds(1);
            t++;
            spawnPoints += t % 2;
            if (t % 3 == Random.Range(0, 3))
                spawnPoints += difficultyLevel;
            if (t % 5 == 0)
                TrySpawnEnemies();
            if (t % 60 == 0)
                difficultyLevel++;
            timeText.text = $"{difficultyLevel}:{t % 60}";
        }
    }

    public void AddPoints(int points)
    {
        spawnPoints += points;
    }
    public void TrySpawnEnemies()
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
}

[System.Serializable]
public class SpawnableEnemy
{
    public EnemyBase enemy;
    public int spawnCost;
    public int minimumDifficulty;
}