using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    GameDataManager gameDataManager;
    void GetStatus()
    {
        moveSpeed = gameDataManager.pMoveSpeed;
        maxX = gameDataManager.pMaxX;
        minX = gameDataManager.pMinX;
        bulletPrefab = gameDataManager.pBulletPrefab;
        bulletInterval = gameDataManager.pBulletInterval;
        bulletPower = gameDataManager.pBulletPower;
    }
    void Update()
    {
        if (!(GameManager.screen == GameManager.Screen.battle)) return;
        // プレイヤーをX軸に移動
        if (Input.GetKey(KeyCode.A)
        || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)
        || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
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
            if (bulletTimer >= bulletInterval)
            {
                Shoot();
                bulletTimer = 0f;
            }
        }
    }

    void Shoot()
    {
        // 弾をプレイヤーの位置に、回転ゼロで生成
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }
}