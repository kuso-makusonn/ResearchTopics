using UnityEngine;

// アイテム効果の基底クラス
public abstract class ItemEffect : ScriptableObject
{
    public bool PurchaseItem(int price)
    {
        bool canPurchase = ItemEffectManager.CanPurchase(price);
        if (canPurchase)
        {
            ApplyEffect();
            AudioManager.instance.PaySound();
            GameDataManager.instance.MoneyUp(-price);
        }
        else
        {
        }
        return canPurchase;
    }
    public abstract void ApplyEffect();
}