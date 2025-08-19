using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/AcceptDrinkingInvitation", fileName = "AcceptDrinkingInvitation")]
public class AcceptDrinkingInvitation : ItemEffect
{
    // 必要な変数を追加
    public float duration;

    public override void ApplyEffect()
    {
        // 効果処理
        ItemEffectManager.AcceptDrinkingInvitation(duration);
    }
}