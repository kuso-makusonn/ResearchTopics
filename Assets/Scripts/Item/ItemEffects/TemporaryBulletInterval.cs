using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/TemporaryBulletInterval", fileName = "TemporaryBulletInterval")]
public class TemporaryBulletInterval : ItemEffect
{
    // 必要な変数を追加
    private int playerIndex = 0;
    public float fact;
    public float duration;

    public override void ApplyEffect()
    {
        // 効果処理
        ItemEffectManager.TemporaryBulletInterval(playerIndex, fact, duration);
    }
}