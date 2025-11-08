using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResultItemController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemNameText, itemEffectText;
    [SerializeField] GameObject highlight;
    public ItemModel itemModel;
    public bool isSuccessItem;

    public void Init(ItemModel _itemModel)
    {
        itemModel = _itemModel;
        Show();
    }
    public void Show()
    {
        itemNameText.text = itemModel.itemName;
        itemImage.sprite = itemModel.itemImage;
        itemEffectText.text = itemModel.effectText;
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
        itemModel.itemEffect.PurchaseItem(0);
        //ここに報酬・バツ画面から離れる処理
        SimulationAttackManager.instance.ExitSimulationAttackResultScreen(isSuccessItem);
    }
}