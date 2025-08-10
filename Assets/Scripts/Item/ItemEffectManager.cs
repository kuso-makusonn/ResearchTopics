using UnityEngine;

public class ItemEffectManager : MonoBehaviour
{
    [SerializeField] GameDataManager gameDataManager;

    public void PowerUp(int amount)
    {
        gameDataManager.pBulletPower += amount;
    }
}
