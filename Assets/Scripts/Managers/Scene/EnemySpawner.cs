using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    [Header("敵のプレハブ")]
    public EnemyManager enemyPrefab;
    public EnemyManager enemyPrefab2;
    public EnemyManager enemyPrefab3;
    public EnemyManager bossPrefab1;
    public float spawnZ;

    [Header("生成間隔の範囲")]
    public float minInterval = 1f;
    public float maxInterval = 3f;
    public float minX = -9f;
    public float maxX = 9f;
    
    public bool canSpawn;
    public bool canBass,canBassSporn;
    public float bassCount;
    public float enemycount;
    private float spawnTimer = 0f;
    private float nextSpawnTime;
    public List<EnemyEntity> enemies = new();
    public EnemyEntity[] allEnemyEntities;
    public EnemyEntity[] allBossEntities;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
    }
    void Start()
    {
        // 最初の生成時間を決定
        SetNextSpawnTime();
        allEnemyEntities = Resources.LoadAll<EnemyEntity>("Enemy/EnemyEntities");
        allBossEntities = Resources.LoadAll<EnemyEntity>("Enemy/BossEntities");
        bassCount = 0;
        enemycount = 0;
        canBassSporn = true;
    }

    void Update()
    {
        if (!canSpawn) return;
        if (!(GameDataManager.instance.screen == GameDataManager.Screen.battle)) return;
        spawnTimer += Time.deltaTime;
        if(enemycount % 15 == 0 && GameDataManager.instance.score > 0 && canBass == false && canBassSporn ==true){
            canBass = true;
            bassCount += 1;
        }
        if (spawnTimer >= nextSpawnTime)
        {
            if(canBass == true){
                SpawnBoss(1);
            }
            else{
                SpawnEnemy(Random.Range(1,allEnemyEntities.Length + 1));
            }
            
            SetNextSpawnTime();
            spawnTimer = 0f;
        }
    }

    void SetNextSpawnTime()
    {
        if(bassCount >= 10){
            if(bassCount >= 20){
                minInterval = 0.2f;
                maxInterval = 1f;
            }
            else{
                minInterval = 0.5f;
                maxInterval = 2f;
            }
            
        }
        nextSpawnTime = Random.Range(minInterval, maxInterval);
    }

    void SpawnEnemy(int id)
    {
        float randomX = Random.Range(minX, maxX);
        if (id == 1)
        {
            Vector3 spawnPosition = new Vector3(randomX, 0f, spawnZ);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            //enemyPrefab.id = id;
        }
        else if (id == 2)
        {
            Vector3 spawnPosition = new Vector3(randomX, 1f, spawnZ);
            Instantiate(enemyPrefab2, spawnPosition, Quaternion.identity);
            //enemyPrefab2.id = id;
        }
        else if (id == 3)
        {
            Vector3 spawnPosition = new Vector3(randomX, 0f, spawnZ);
            Instantiate(enemyPrefab3, spawnPosition, Quaternion.identity);
            //enemyPrefab3.id = id;
        }
        
    }
    void SpawnBoss(int id)
    {
        AudioManager.instance.Danger();
        if (id == 1)
        {
            Vector3 spawnPosition = new Vector3(0, 0f, spawnZ);
            Instantiate(bossPrefab1, spawnPosition, Quaternion.identity);
        }
        canBass = false;
        canBassSporn = false;
    }
    public int CreateNewEnemyData(int id)
    {
        EnemyEntity enemy = Resources.Load<EnemyEntity>("Enemy/EnemyEntities/Enemy" + id);
        enemies.Add(enemy);
        return enemies.Count - 1;
    }
    public int CreateNewBossData(int id)
    {
        EnemyEntity enemy = Resources.Load<EnemyEntity>("Enemy/BossEntities/Boss" + id);
        enemies.Add(enemy);
        return enemies.Count - 1;
    }
}