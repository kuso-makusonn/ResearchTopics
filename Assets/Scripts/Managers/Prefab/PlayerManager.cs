using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerData playerData;
    [SerializeField] GameObject defaultBulletPrefab;
    public float shootingHeight;

    private bool isShooting;
    private float bulletTimer;
    private int playerIndex;

    public Camera mainCamera;       // 照準に使うカメラ（通常はMain Camera）
    public LayerMask aimLayer;      // 照準を当てる対象のレイヤー（例：目に見えないPlane）
    Quaternion onlyY;

    void Start()
    {
        playerIndex = GameDataManager.instance.CreateNewPlayerData();
        playerData = GameDataManager.instance.players[playerIndex];
        playerData.bulletPrefab = defaultBulletPrefab;
    }
    void Update()
    {
        if (!(GameDataManager.instance.screen == GameDataManager.Screen.battle)) return;
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

        // マウス位置からワールド空間へレイを飛ばす
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // レイキャストで「照準面」に当たったかチェック
        if (Physics.Raycast(ray, out hit, 1000f, aimLayer))
        {
            Vector3 targetPoint = hit.point; // 当たった座標が狙うポイント

            // ターゲット方向を計算
            Vector3 dir = (targetPoint - transform.position).normalized;

            // LookRotationで方向をQuaternionに変換
            Quaternion lookRot = Quaternion.LookRotation(dir);

            // Y軸だけ残して、XとZを無視
            onlyY = Quaternion.Euler(0, lookRot.eulerAngles.y, 0);
        }

        // スペースキーが押されているかを確認
        if (Input.GetKey(KeyCode.Space)
        || Input.GetKey(KeyCode.W)
        || Input.GetMouseButton(0))
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
                //Shoot();
                GoShoot();
                bulletTimer = 0f;
            }
        }
    }

    void GoShoot()
    {
        // 弾を生成（水平だけ向いた状態で発射）
        Instantiate(playerData.bulletPrefab, transform.position + new Vector3(0, shootingHeight, 0), onlyY);
    }
}