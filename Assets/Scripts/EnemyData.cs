using UnityEngine;

public class EnemyData
{
    public float moveSpeed;
    public int hp;
    public float minZ;
    public void InitData()
    {
        moveSpeed = 10f;
        hp = 3;
        minZ = 0;
    }
}