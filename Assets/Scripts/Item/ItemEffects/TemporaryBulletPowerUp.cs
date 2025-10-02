using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/TemporaryBulletPowerUp", fileName = "TemporaryBulletPowerUp")]
public class TemporaryBulletPowerUp : ItemEffect
{
    // 必要な変数を追加
    private int playerIndex = 0;
    public int amount;
    public float duration;

    public override void ApplyEffect()
    {
        // 効果処理
        ItemEffectManager.TemporaryBulletPowerUp(playerIndex, amount, duration);
    }
}