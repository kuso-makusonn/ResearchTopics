using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ItemEffectManager ItemEffectManager;
    [SerializeField] GameObject shop, itemArea, itemPrefab;
    List<ItemController> itemList = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!(GameManager.screen == GameManager.Screen.shop)) return;
    }
    public void EnterShop()
    {
        shop.SetActive(true);
        itemList = new();
        SetItems();

        void SetItems()
        {
            RectTransform rect = itemArea.transform as RectTransform;
            for (int i = itemArea.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(itemArea.transform.GetChild(i).gameObject);
            }
            ItemEntity[] allItemEntities = Resources.LoadAll<ItemEntity>("Items/ItemEntities");
            foreach (ItemEntity itemEntity in allItemEntities)
            {
                ItemController item = Instantiate(itemPrefab, itemArea.transform).GetComponent<ItemController>();
                item.Init(itemEntity, ItemEffectManager);
                itemList.Add(item);
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
        itemList = null;
        shop.SetActive(false);
    }
}
