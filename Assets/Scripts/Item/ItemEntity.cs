using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create New Item")]
public class ItemEntity : ScriptableObject
{
    public int itemId;
    public string itemName;
    public Sprite itemImage;
    public int price;
    public string effectText;
    public ItemEffect itemEffect;
}