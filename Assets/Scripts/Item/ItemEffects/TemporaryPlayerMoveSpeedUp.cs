using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/TemporaryPlayerMoveSpeedUp", fileName = "TemporaryPlayerMoveSpeedUp")]
public class TemporaryPlayerMoveSpeedUp : ItemEffect
{
    // 必要な変数を追加
    private int playerIndex = 0;
    public float fact;
    public float duration;

    public override void ApplyEffect()
    {
        // 効果処理
        ItemEffectManager.TemporaryPlayerMoveSpeedUp(playerIndex, fact, duration);
    }
}