using UnityEngine;

public class PlayerData
{
    public float moveSpeed;
    public float maxX;
    public float minX;
    public GameObject bulletPrefab;
    public float bulletInterval;
    public float bulletPower;
    public void InitData()
    {
        moveSpeed = 10f;
        maxX = 9f;
        minX = -9f;
        bulletInterval = 0.1f;
        bulletPower = 1f;
    }
}