using System.Collections.Generic;

[System.Serializable]
public class MailModel
{
    public string title, sender, main, link;
    public bool isDark, isPhishing;
    public List<DiscountItemTemplate> discountItems;
    [System.Serializable]
    public struct DiscountItemTemplate
    {
        public ItemEntity itemEntity;
        public int discount;
        public int maxPurchaseCount;
    }
    public MailModel(MailEntity mailEntity)
    {
        title = mailEntity.title;
        sender = mailEntity.sender;
        main = mailEntity.main;
        link = mailEntity.link;
        discountItems = mailEntity.discountItems;
    }
}
