using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    public Enemy[] enemies;

    bool isGameEnded;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].enemyPool = new Queue<GameObject>();
        }

        GameManager.Instance.OnGameStart += StartSpawning;
        GameManager.Instance.OnPlayerDeath += StopSpawning;
    }

    public void StartSpawning()
    {
        isGameEnded = false;

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].enemyPool.Clear();

            for (int j = 0; j < enemies[i].amount; j++)
            {
                GameObject tmp = Instantiate(enemies[i].enemyPrefab);
                enemies[i].enemyPool.Enqueue(tmp);
            }
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            StartCoroutine(Spawning(i));
        }
    }

    IEnumerator Spawning(int i)
    {
        Enemy enemy = enemies[i];
        float timer = 0f;
        Vector3 spawnPoint;
        GameObject spawnedEnemy;
        while(timer < enemy.timeBeforeSpawn)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        timer = enemy.timeIntervalBetweenSpawns;
        while(!isGameEnded)
        {
            timer += Time.deltaTime;
            if(timer >= enemy.timeIntervalBetweenSpawns && enemy.enemyPool.Count > 0)
            {
                timer = 0f;
                spawnPoint = enemy.spawnPoints[Random.Range(0, enemy.spawnPoints.Length - 1)];
                spawnedEnemy = enemy.enemyPool.Dequeue();
                spawnedEnemy.transform.position = spawnPoint;
                spawnedEnemy.SetActive(true);
            }
            yield return null;
        }
    }

    public void StopSpawning()
    {
        isGameEnded = true;
    }

    public void ReturnEnemy(GameObject enemy, int id)
    {
        bool found = false;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].id == id)
            {
                found = true;
                enemies[i].enemyPool.Enqueue(enemy);
                break;
            }
        }
        if(!found)
        {
            Debug.Log("No matching ID!");
            return;
        }
    }
}