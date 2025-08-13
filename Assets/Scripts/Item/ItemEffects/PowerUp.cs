using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/PowerUp", fileName = "PowerUp")]
public class PowerUp : ItemEffect
{
    // 必要な変数を追加
    public float boostMultiplier;
    public float duration;

    public override void ApplyEffect(ItemEffectManager itemEffectManager)
    {
        // 効果処理
        itemEffectManager.StartCoroutine(itemEffectManager.BoostAttack(boostMultiplier, duration));
    }
}