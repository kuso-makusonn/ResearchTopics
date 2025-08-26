using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    [Header("敵のプレハブ")]
    public GameObject enemyPrefab;
    public float spawnZ;

    [Header("生成間隔の範囲")]
    public float minInterval = 1f;
    public float maxInterval = 3f;
    public float minX = -9f;
    public float maxX = 9f;

    private float spawnTimer = 0f;
    private float nextSpawnTime;
    public List<EnemyData> enemies = new();
    public bool canSpawn;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
    }
    void Start()
    {
        // 最初の生成時間を決定
        SetNextSpawnTime();
    }

    void Update()
    {
        if (!canSpawn) return;
        if (!(GameDataManager.instance.screen == GameDataManager.Screen.battle)) return;
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= nextSpawnTime)
        {
            SpawnEnemy();
            SetNextSpawnTime();
            spawnTimer = 0f;
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minInterval, maxInterval);
    }

    void SpawnEnemy()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, 0f, spawnZ);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
    public int CreateNewEnemyData()
    {
        EnemyData enemy = new();
        enemy.InitData();
        enemies.Add(enemy);
        return enemies.Count - 1;
    }
}