using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/MoneyUp", fileName = "MoneyUp")]
public class MoneyUp : ItemEffect
{
    // 必要な変数を追加
    public int amount;

    public override void ApplyEffect()
    {
        // 効果処理
        ItemEffectManager.MoneyUp(amount);
    }
}