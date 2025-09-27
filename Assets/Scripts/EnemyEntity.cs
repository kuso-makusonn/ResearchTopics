using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Create New Enemy")]
public class EnemyEntity : ScriptableObject
{
    public float moveSpeed;
    public float hp;
    public float minZ;
}
