using UnityEngine;

// アイテム効果の基底クラス
public abstract class ItemEffect : ScriptableObject
{
    public void PurchaseItem(ItemEffectManager itemEffectManager, int price)
    {
        if (itemEffectManager.CanPurchase(price)) ApplyEffect(itemEffectManager);
        else Debug.Log("お前とは飲みに行かねぇよ"); ;
    }
    public abstract void ApplyEffect(ItemEffectManager itemEffectManager);
}