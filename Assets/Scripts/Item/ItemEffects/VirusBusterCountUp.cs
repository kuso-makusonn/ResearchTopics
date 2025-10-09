using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/VirusBusterCountUp", fileName = "VirusBusterCountUp")]
public class VirusBusterCountUp : ItemEffect
{
    // 必要な変数を追加
    public int amount;

    public override void ApplyEffect()
    {
        // 効果処理
        ItemEffectManager.VirusBusterCountUp(amount);
    }
}