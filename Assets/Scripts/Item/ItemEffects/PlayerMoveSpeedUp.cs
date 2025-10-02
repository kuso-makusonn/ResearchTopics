using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/PlayerMoveSpeedUp", fileName = "PlayerMoveSpeedUp")]
public class PlayerMoveSpeedUp : ItemEffect
{
    // 必要な変数を追加
    private int playerIndex = 0;
    public float fact;

    public override void ApplyEffect()
    {
        // 効果処理
        ItemEffectManager.PlayerMoveSpeedUp(playerIndex, fact);
    }
}