using System.Collections;
using UnityEngine;


public class ItemEffectManager : MonoBehaviour
{
    [SerializeField] GameDataManager gameDataManager;

    public bool CanPurchase(int price)
    {
        return gameDataManager.money >= price;
    }
    public IEnumerator BoostAttack(float boostMultiplier, float duration)
    {
        yield return null;
        Debug.Log("おぅ、飲み行こ飲み行こ");
    }
}
