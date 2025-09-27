using UnityEngine;

// アイテム効果の基底クラス
public abstract class ItemEffect : ScriptableObject
{
    public void PurchaseItem(int price)
    {
        if (ItemEffectManager.CanPurchase(price)) {
            ApplyEffect();
            GameDataManager.instance.MoneyUp(-1*price);
        }
        else Debug.Log("お前とは飲みに行かねぇよ");
    }
    public abstract void ApplyEffect();
}