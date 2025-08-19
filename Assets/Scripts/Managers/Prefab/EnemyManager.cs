using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyData enemyData;
    private int enemyIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyIndex = EnemySpawner.instance.CreateNewEnemyData();
        enemyData = EnemySpawner.instance.enemies[enemyIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (!(GameDataManager.instance.screen == GameDataManager.Screen.battle)) return;
        transform.Translate(Vector3.back * enemyData.moveSpeed * Time.deltaTime);
        if (transform.position.z < enemyData.minZ)
        {
            Debug.Log("GameOver");
            Destroy(gameObject);
            GameManager.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            enemyData.hp--;
            if (enemyData.hp <= 0)
            {
                Destroy(gameObject);
                GameDataManager.instance.ScoreUp(1);
                GameDataManager.instance.MoneyUp(100);
            }
        }
    }
}
