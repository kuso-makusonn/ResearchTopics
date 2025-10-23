using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyManager : MonoBehaviour
{
    public int id;
    public EnemyEntity enemyData;
    [SerializeField] Animator animator;
    private int enemyIndex;
    public float hp;
    public bool isBoss;
    public float bigger;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(isBoss == false){
            enemyIndex = EnemySpawner.instance.CreateNewEnemyData(id);
            bigger = EnemySpawner.instance.bassCount + 1;
        }
        else{
            enemyIndex = EnemySpawner.instance.CreateNewBossData(id);
            bigger = EnemySpawner.instance.bassCount;
        }
        enemyData = EnemySpawner.instance.enemies[enemyIndex];
        if(EnemySpawner.instance.bassCount > 10){
            bigger = bigger * 5;
        }
        hp = enemyData.hp * bigger;
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
            AudioManager.instance.DamageSound();
            hp -= BulletManager.bulletPower;
            if (hp <= 0)
            {
                Destroy(gameObject);
                if(EnemySpawner.instance.bassCount >= 10){
                    if(isBoss == false){
                        GameDataManager.instance.ScoreUp(5);
                        GameDataManager.instance.MoneyUp(500);
                    }
                    else{
                        AudioManager.instance.NotDanger();
                        GameDataManager.instance.ScoreUp(10);
                        GameDataManager.instance.MoneyUp(7500);
                        EnemySpawner.instance.canBassSporn = true;
                    }
                }
                else{
                    if(isBoss == false){
                        GameDataManager.instance.ScoreUp(1);
                        GameDataManager.instance.MoneyUp(100);
                    }
                    else{
                        AudioManager.instance.NotDanger();
                        GameDataManager.instance.ScoreUp(5);
                        GameDataManager.instance.MoneyUp(1500);
                        EnemySpawner.instance.canBassSporn = true;
                    }
                }
                EnemySpawner.instance.enemycount += 1;
            }
        }
    }
}
