using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 100f;
    public int hp = 3;
    public float minZ;
    public GameDataManager gameDataManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!(GameManager.screen == GameManager.Screen.battle)) return;
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        if (transform.position.z < minZ)
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
            hp--;
            if (hp <= 0)
            {
                Destroy(gameObject);
                gameDataManager.ScoreUp(1);
                gameDataManager.MoneyUp(100);
            }
        }
    }
}
