using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyManager : MonoBehaviour
{
    public int id;
    public EnemyEntity enemyData;
    [SerializeField] Animator animator;
    private int enemyIndex;
    public int hp;
    public bool isBoss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(isBoss == false){
            enemyIndex = EnemySpawner.instance.CreateNewEnemyData(id);
        }
        else{
            enemyIndex = EnemySpawner.instance.CreateNewBossData(id);
        }
        enemyData = EnemySpawner.instance.enemies[enemyIndex];
        hp = enemyData.hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(GameDataManager.instance.screen == GameDataManager.Screen.battle))
        {
            animator.speed = 0f;
            return;
        }
        animator.speed = 1f;
        transform.Translate(Vector3.back * enemyData.moveSpeed * Time.deltaTime);
        if (transform.position.z < enemyData.minZ)
        {
            Debug.Log("GameOver");
            Destroy(gameObject);
            GameManager.instance.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            hp--;
            if (hp <= 0)
            {
                Destroy(gameObject);
                if(isBoss == false){
                    GameDataManager.instance.ScoreUp(1);
                    GameDataManager.instance.MoneyUp(100);
                }
                else{
                    GameDataManager.instance.ScoreUp(1);
                    GameDataManager.instance.MoneyUp(1500);
                    EnemySpawner.instance.canBass =true;
                }
            }
        }
    }
}
