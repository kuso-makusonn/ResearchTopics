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
    int price;

    public void Init(ItemEntity itemEntity)
    {
        itemNameText.text = itemEntity.itemName;
        itemImage.sprite = itemEntity.itemImage;
        price = itemEntity.price;
        priceText.text = "価格:" + price + "円";
        itemEffectText.text = itemEntity.effectText;
        itemEffect = itemEntity.itemEffect;
        highlight.SetActive(false);
    }
    public void HighlightItem(bool isHighlight)
    {
        highlight.SetActive(isHighlight);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        highlight.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        highlight.SetActive(false);
    }
    public void ItemClickButton()
    {
        itemEffect.PurchaseItem(price);
    }
}
