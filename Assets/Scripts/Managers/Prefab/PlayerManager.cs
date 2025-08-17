using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerData playerData;
    [SerializeField] GameDataManager gameDataManager;
    [SerializeField] GameObject defaultBulletPrefab;
    public float shootingHeight;

    private bool isShooting;
    private float bulletTimer;
    private int playerIndex;

    void Start()
    {
        playerIndex = gameDataManager.CreateNewPlayerData();
        playerData = gameDataManager.players[playerIndex];
        playerData.bulletPrefab = defaultBulletPrefab;
    }
    void Update()
    {
        if (!(GameManager.screen == GameManager.Screen.battle)) return;
        // プレイヤーをX軸に移動
        if (Input.GetKey(KeyCode.A)
        || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * playerData.moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)
        || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * playerData.moveSpeed * Time.deltaTime);
        }
        if (transform.position.x > playerData.maxX)
        {
            transform.position = new Vector3(playerData.maxX, transform.position.y, transform.position.z);
        }
        if (transform.position.x < playerData.minX)
        {
            transform.position = new Vector3(playerData.minX, transform.position.y, transform.position.z);
        }

        // スペースキーが押されているかを確認
        if (Input.GetKey(KeyCode.Space)
        || Input.GetKey(KeyCode.W))
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
            bulletTimer = 0f; // 離したときにタイマーをリセット
        }

        // 弾の連射処理
        if (isShooting)
        {
            bulletTimer += Time.deltaTime;
            if (bulletTimer >= playerData.bulletInterval)
            {
                Shoot();
                bulletTimer = 0f;
            }
        }
    }

    void Shoot()
    {
        // 弾をプレイヤーの位置に、回転ゼロで生成
        Instantiate(playerData.bulletPrefab, transform.position + new Vector3(0,shootingHeight,0), Quaternion.identity);
    }
}