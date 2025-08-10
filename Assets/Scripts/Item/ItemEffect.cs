using UnityEngine;

// アイテム効果の基底クラス
public abstract class ItemEffect : ScriptableObject
{
    public abstract void ApplyEffect(ItemEffectManager itemEffectManager);
}