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
    int price;

    public void Init(ItemEntity itemEntity, ItemEffectManager _itemEffectManager)
    {
        itemNameText.text = itemEntity.itemName;
        itemImage.sprite = itemEntity.itemImage;
        price = itemEntity.price;
        priceText.text = "価格:" + price + "円";
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
        itemEffect.PurchaseItem(itemEffectManager, price);
    }
}
