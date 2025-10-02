using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/BulletInterval", fileName = "BulletInterval")]
public class BulletInterval : ItemEffect
{
    // 必要な変数を追加
    private int playerIndex = 0;
    public float amount;

    public override void ApplyEffect()
    {
        // 効果処理
        ItemEffectManager.BulletInterval(playerIndex, amount);
    }
}