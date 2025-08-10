using UnityEngine;

public class PlayerData
{
    [Header("移動設定")]
    public float pMoveSpeed { get; private set; } = 10f;
    public float pMaxX { get; private set; } = 5f;
    public float pMinX { get; private set; } = -5f;

    [Header("弾の設定")]
    public GameObject pBulletPrefab { get; private set; }
    public float pBulletInterval { get; private set; } = 0.3f;
    public float pBulletPower { get; private set; } = 1;
}