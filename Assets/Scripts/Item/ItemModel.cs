using UnityEngine;

[System.Serializable]
public class ItemModel
{
    public int itemId;
    public string itemName;
    public Sprite itemImage;
    public int price;
    public string effectText;
    public ItemEffect itemEffect;

    public int discount = 0;
    public int maxPurchaseCount = -1;

    public bool isHighlight = false;
    public int purchaseCount = 0;
    public ItemModel(ItemEntity itemEntity)
    {
        itemId = itemEntity.itemId;
        itemName = itemEntity.itemName;
        itemImage = itemEntity.itemImage;
        price = itemEntity.price;
        effectText = itemEntity.effectText;
        itemEffect = itemEntity.itemEffect;
    }
}
