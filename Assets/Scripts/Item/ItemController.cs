using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemNameText, priceText, itemEffectText;
    [SerializeField] GameObject highlight;
    public ItemModel itemModel;

    public void Init(ItemModel _itemModel)
    {
        itemModel = _itemModel;
        Show();
    }
    public void Show()
    {
        itemNameText.text = itemModel.itemName;
        itemImage.sprite = itemModel.itemImage;
        priceText.text = "価格:" + Price() + "円";
        highlight.SetActive(itemModel.isHighlight);
    }
    private int Price()
    {
        return itemModel.price * (100 - itemModel.discount);
    }
    public void HighlightItem(bool isHighlight)
    {
        itemModel.isHighlight = isHighlight;
        highlight.SetActive(isHighlight);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        HighlightItem(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HighlightItem(false);
    }
    public void ItemClickButton()
    {
        itemModel.purchaseCount++;
        if (itemModel.maxPurchaseCount > 0
        && itemModel.purchaseCount >= itemModel.maxPurchaseCount)
        {
            ShopManager.instance.RemoveItem(itemModel);
            Destroy(gameObject);
        }
        itemModel.itemEffect.PurchaseItem(Price());
    }
}
