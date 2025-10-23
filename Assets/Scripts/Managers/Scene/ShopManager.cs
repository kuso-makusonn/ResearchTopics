using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);

        allItemEntities = Resources.LoadAll<ItemEntity>("Items/ItemEntities");
    }
    private void OnApplicationQuit()
    {
        ItemEffectManager.CancelAllEffect();
    }

    [SerializeField] GameObject shopZoom, shopNotZoom, itemArea, itemPrefab;
    [SerializeField] List<ItemModel> itemList = new();
    ItemEntity[] allItemEntities;

    private void Start()
    {
        foreach (ItemEntity itemEntity in allItemEntities)
        {
            itemList.Add(new(itemEntity));
        }
    }
    public void AddItem(ItemModel itemModel)
    {
        itemList.Add(itemModel);
    }
    public void RemoveItem(ItemModel itemModel)
    {
        itemList.Remove(itemModel);
    }
    public void EnterShop()
    {
        StartCoroutine(SetActiveExtension.Zoom(shopZoom, true));
        shopNotZoom.SetActive(true);
        SetItems();

        void SetItems()
        {
            RectTransform rect = itemArea.transform as RectTransform;
            for (int i = itemArea.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(itemArea.transform.GetChild(i).gameObject);
            }
            for (int i = itemList.Count - 1; i >= 0; i--)
            {
                ItemController item = Instantiate(itemPrefab, itemArea.transform).GetComponent<ItemController>();
                item.Init(itemList[i]);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            itemArea.transform.position = new Vector3(
                itemArea.transform.position.x,
                -rect.rect.height / 2,
                itemArea.transform.position.z
            );
        }
    }
    public void ExitShop()
    {
        StartCoroutine(SetActiveExtension.Zoom(shopZoom, false));
        shopNotZoom.SetActive(false);
    }
}
