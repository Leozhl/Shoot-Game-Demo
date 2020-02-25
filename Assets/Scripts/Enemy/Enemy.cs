using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Demo/Enemy", order = 1)]
public class Enemy : ScriptableObject
{
    public GameObject enemyPrefab;
    public int amount;
    public int id;
    public float timeBeforeSpawn;
    public float timeIntervalBetweenSpawns;
    public Vector3[] spawnPoints;
    public Queue<GameObject> enemyPool;
}