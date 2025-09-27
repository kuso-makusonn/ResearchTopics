using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/AcceptDrinkingInvitation", fileName = "AcceptDrinkingInvitation")]
public class AcceptDrinkingInvitation : ItemEffect
{
    // 必要な変数を追加
    public float duration;
    public float powerplus,speedup;
    public bool power,speed;

    public override void ApplyEffect()
    {
        // 効果処理
        ItemEffectManager.AcceptDrinkingInvitation(duration, powerplus,speedup,power,speed);
    }
}