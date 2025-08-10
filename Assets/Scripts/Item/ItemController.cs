using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemNameText, priceText, itemEffectText;
    [SerializeField] GameObject highlight;
    ItemEffect itemEffect;
    ItemEffectManager itemEffectManager;
    public void Init(ItemEntity itemEntity, ItemEffectManager _itemEffectManager)
    {
        itemNameText.text = itemEntity.itemName;
        itemImage.sprite = itemEntity.itemImage;
        priceText.text = "価格:" + itemEntity.price + "円";
        itemEffectText.text = itemEntity.effectText;
        itemEffect = itemEntity.itemEffect;
        highlight.SetActive(false);
        itemEffectManager = _itemEffectManager;
    }
    public void HighlightItem(bool isHighlight)
    {
        highlight.SetActive(isHighlight);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        highlight.SetActive(true);
    }

    // カーソルが対象UIの上から離れた時に呼ばれる
    public void OnPointerExit(PointerEventData eventData)
    {
        highlight.SetActive(false);
    }
    public void ItemClickButton()
    {
        Debug.Log("おぅ、飲み行こ飲み行こ");
        itemEffect.ApplyEffect(itemEffectManager);
    }
}
