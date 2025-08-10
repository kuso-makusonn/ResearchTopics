using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/PowerUp", fileName = "PowerUp")]
public class PowerUp : ItemEffect
{
    // 必要な変数を追加
    int amount;

    public override void ApplyEffect(ItemEffectManager itemEffectManager)
    {
        // 効果処理
        itemEffectManager.PowerUp(amount);
    }
}