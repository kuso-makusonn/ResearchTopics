using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 100f;
    public float bulletPower = 1;
    public int maxZ = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!(GameDataManager.instance.screen == GameDataManager.Screen.battle)) return;
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        if (transform.position.z > maxZ)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}