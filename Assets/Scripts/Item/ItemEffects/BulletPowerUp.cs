using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/BulletPowerUp", fileName = "BulletPowerUp")]
public class BulletPowerUp : ItemEffect
{
    // 必要な変数を追加
    private int playerIndex = 0;
    public int amount;

    public override void ApplyEffect()
    {
        // 効果処理
        ItemEffectManager.BulletPowerUp(playerIndex, amount);
    }
}