using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ItemEffectManager ItemEffectManager;
    [SerializeField] GameObject shop, itemArea, itemPrefab;
    [SerializeField] LayoutGroup layoutGroup;
    List<ItemController> itemList = new();
    ItemController[] itemView = new ItemController[0];
    int selectNumber;
    int highlightNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!(GameManager.screen == GameManager.Screen.shop)) return;
        // if (Input.GetKeyDown(KeyCode.RightArrow) || !Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     SelectRightItem();
        // }
        // if (Input.GetKeyDown(KeyCode.LeftArrow) || !Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     SelectLeftItem();
        // }
    }
    public void EnterShop()
    {
        shop.SetActive(true);
        itemList = new();
        itemView = new ItemController[5];
        SetItems();
        // SelectItem(1);

        void SetItems()
        {
            RectTransform rect = itemArea.transform as RectTransform;
            for (int i = itemArea.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(itemArea.transform.GetChild(i).gameObject);
            }
            ItemEntity[] allItemEntities = Resources.LoadAll<ItemEntity>("Items");
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
    // void SelectItem(int number)
    // {
    //     if (number < 1 || itemList.Count < number) return;
    //     int index = number - 1;
    //     if (number <= 2)
    //     {
    //     }
    //     if (number >= itemList.Count - 1)
    //         selectNumber = number;
    // }
    // void SelectRightItem()
    // {
    //     if (selectNumber >= itemList.Count) return;
    //     if (highlightNumber == 3)
    //     {
    //     }
    //     else
    //     {
    //         HighlightMove(1);
    //     }
    //     SelectItem(selectNumber + 1);
    // }
    // void SelectLeftItem()
    // {
    //     if (selectNumber <= 1) return;
    //     if (highlightNumber == 1)
    //     {
    //     }
    //     else
    //     {
    //         HighlightMove(-1);
    //     }
    //     SelectItem(selectNumber - 1);
    // }
    // void HighlightMove(int amount)
    // {
    //     highlightNumber += amount;
    //     if (highlightNumber < 1)
    //     {
    //         highlightNumber = 1;
    //     }
    //     if (highlightNumber > 3)
    //     {
    //         highlightNumber = 3;
    //     }
    // }
}
