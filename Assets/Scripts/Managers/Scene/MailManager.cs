using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailManager : MonoBehaviour
{
    public static MailManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
    }


    [SerializeField] GameObject mailDefaultZoom, mailListZoom, mailListNotZoom, mailArea, mailPrefab, sendedMail, mailDetailZoom, mailDetailNotZoom;
    [SerializeField] ScrollRect mailListScrollRect;
    [SerializeField] RectTransform mailDetailContent, mailDetailBackground, title, sender, main, linkButton, link;
    [SerializeField] TextMeshProUGUI titleText, senderText, mainText, linkText;
    public List<MailModel> mailList = new();
    public MailEntity[] allMailEntities;
    public MailEntity[] allPhishingMailEntities;
    public MailEntity[] allSuccessRewardMailEntities;
    public bool canSendMail;
    private bool isSendingMail;
    private float sendMailTimer;
    private float nextSendMailTime;
    public bool isPhishingMailAttacking;
    private MailModel nowMailDetailModel;

    private void Start()
    {
        allMailEntities = Resources.LoadAll<MailEntity>("Mails/MailEntities");
        allPhishingMailEntities = Resources.LoadAll<MailEntity>("Mails/PhishingMailEntities");
        allSuccessRewardMailEntities = Resources.LoadAll<MailEntity>("Mails/SuccessRewardMailEntities");
        SetNextSendMailTime();
        NewMail(0);
        isPhishingMailAttacking = true;
        NewPhishingMail(0);
    }
    private void Update()
    {
        if (!canSendMail) return;
        if (isSendingMail) return;
        if (GameDataManager.instance.screen != GameDataManager.Screen.battle) return;

        // ゲームが動いている間のみカウントアップ
        sendMailTimer += Time.deltaTime;

        if (sendMailTimer >= nextSendMailTime)
        {
            SendNewMail(isPhishingMailAttacking);
            Debug.Log("新しいメールが届きました");
        }
    }
    private void SetNextSendMailTime()
    {
        nextSendMailTime = UnityEngine.Random.Range(10, 20);
        sendMailTimer = 0f;
        isSendingMail = false;
        sendedMail.SetActive(false);
    }
    public void CheckSendedMailButton()
    {
        GameManager.instance.MenuButton();
        GameManager.instance.MailButton();
        mailListScrollRect.verticalNormalizedPosition = 1f;
        ShowMailDetail(mailList[mailList.Count - 1]);

        SetNextSendMailTime();
    }
    public void ShowMailList()
    {
        StartCoroutine(SetActiveExtension.Zoom(sendedMail, false));

        StartCoroutine(SetActiveExtension.Zoom(mailDefaultZoom, true));
        StartCoroutine(SetActiveExtension.Zoom(mailListZoom, true));
        mailListNotZoom.SetActive(true);
        StartCoroutine(SetActiveExtension.Zoom(mailDetailZoom, false));
        mailDetailNotZoom.SetActive(false);
        nowMailDetailModel = null;
        SetMails();

        void SetMails()
        {
            for (int i = mailArea.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(mailArea.transform.GetChild(i).gameObject);
            }
            for (int i = mailList.Count - 1; i >= 0; i--)
            {
                MailController newMailController = Instantiate(mailPrefab, mailArea.transform).GetComponent<MailController>();
                newMailController.Init(mailList[i]);
            }
        }
    }
    public void ExitMail()
    {
        StartCoroutine(SetActiveExtension.Zoom(mailDefaultZoom, false));
        StartCoroutine(SetActiveExtension.Zoom(mailListZoom, false));
        mailListNotZoom.SetActive(false);
        StartCoroutine(SetActiveExtension.Zoom(mailDetailZoom, false));
        mailDetailNotZoom.SetActive(false);
    }
    private void NewMail(int index)
    {
        if (index < 0 || index > allMailEntities.Length - 1)
        {
            Debug.Log("そんなメールは無えょ！");
            return;
        }
        MailEntity mailEntity = Instantiate(allMailEntities[index]);
        MailModel mailModel = new(mailEntity);
        mailModel.isPhishing = false;
        mailList.Add(mailModel);
    }
    private void NewPhishingMail(int index)
    {
        if (!isPhishingMailAttacking) return;
        if (index < 0 || index > allPhishingMailEntities.Length - 1)
        {
            Debug.Log("そんなメールは無えょ！");
            return;
        }
        isPhishingMailAttacking = false;
        MailEntity mailEntity = Instantiate(allPhishingMailEntities[index]);
        MailModel mailModel = new(mailEntity);
        mailModel.isPhishing = true;
        mailList.Add(mailModel);
    }
    public void NewSuccessRewardMail()
    {
        int index = Random.Range(0, allSuccessRewardMailEntities.Length - 1);
        MailEntity mailEntity = Instantiate(allSuccessRewardMailEntities[index]);
        MailModel mailModel = new(mailEntity);
        mailModel.isPhishing = false;
        mailList.Add(mailModel);
    }
    public void DeleteMail(int index)
    {
        if (index < 0 || index > mailList.Count - 1)
        {
            Debug.Log("そんなメールは無えょ！");
            return;
        }
        mailList.RemoveAt(index);
    }
    private void SendNewMail(bool isPhishing)
    {
        sendedMail.SetActive(true);
        isSendingMail = true;
        if (isPhishing)
        {
            NewPhishingMail(UnityEngine.Random.Range(0, allPhishingMailEntities.Length - 1));
        }
        else
        {
            NewMail(UnityEngine.Random.Range(0, allMailEntities.Length - 1));
        }
        SetNextSendMailTime();
    }
    public void ShowMailDetail(MailModel mailModel)
    {
        if (mailModel.isPhishing)
        {
            SimulationAttackManager.instance.StartResponseTime();
        }
        nowMailDetailModel = mailModel;
        mailListNotZoom.SetActive(false);
        StartCoroutine(SetActiveExtension.Zoom(mailListZoom, false));
        mailDetailNotZoom.SetActive(true);
        StartCoroutine(SetActiveExtension.Zoom(mailDetailZoom, true));
        titleText.text = mailModel.title;
        senderText.text = $"From:{mailModel.sender}";
        mainText.text = mailModel.main;
        linkText.text = mailModel.link;

        title.sizeDelta = new Vector2(title.sizeDelta.x, titleText.preferredHeight);
        sender.sizeDelta = new Vector2(sender.sizeDelta.x, senderText.preferredHeight);
        main.sizeDelta = new Vector2(main.sizeDelta.x, mainText.preferredHeight);
        link.sizeDelta = new Vector2(link.sizeDelta.x, linkText.preferredHeight);
        linkButton.sizeDelta = new Vector2(linkButton.sizeDelta.x, linkText.preferredHeight);

        float top = 20f;
        float bottom = 20f;
        float spacing = 20f;

        float currentY = top;
        for (int i = 0; i < mailDetailBackground.childCount; i++)
        {
            RectTransform rt = mailDetailBackground.GetChild(i).GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -currentY);
            float height = rt.sizeDelta.y;
            currentY += height + spacing;
        }

        float totalHeight = currentY - spacing + bottom;
        mailDetailBackground.sizeDelta = new Vector2(mailDetailBackground.sizeDelta.x, totalHeight);

        mailDetailContent.sizeDelta = new Vector2(mailDetailBackground.sizeDelta.x, totalHeight);
    }
    public void LinkButton()
    {
        if (nowMailDetailModel == null) return;
        if (nowMailDetailModel.isPhishing)
        {
            SimulationAttackManager.instance.PhishingEnd(false);
            nowMailDetailModel.isDark = true;
            ShowMailList();
        }
        else
        {
            foreach (MailModel.DiscountItemTemplate item in nowMailDetailModel.discountItems)
            {
                ItemModel itemModel = new(item.itemEntity);
                itemModel.discount = item.discount;
                itemModel.maxPurchaseCount = item.maxPurchaseCount;

                ShopManager.instance.AddItem(itemModel);
            }
            nowMailDetailModel.isDark = true;
            mailDefaultZoom.SetActive(false);
            mailListZoom.SetActive(false);
            mailListNotZoom.SetActive(false);
            mailDetailZoom.SetActive(false);
            mailDetailNotZoom.SetActive(false);
            GameManager.instance.ToShopButton();
        }
    }
    public void WarningButton()
    {
        if (nowMailDetailModel == null) return;
        if (nowMailDetailModel.isPhishing)
        {
            SimulationAttackManager.instance.PhishingEnd(true);
            nowMailDetailModel.isDark = true;
            ShowMailList();
        }
        else if (!nowMailDetailModel.isPhishing)
        {
            Debug.Log("これは正当なメールです");
        }
        else
        {
            Debug.Log("メールが見つかりません");
        }
    }
    public void ReturnMenu()
    {
        GameManager.instance.ReturnToMenu();
    }
    public void ExitMailDetail()
    {
        if (nowMailDetailModel == null) return;
        if (nowMailDetailModel.isPhishing)
        {
            SimulationAttackManager.instance.PhishingEnd(false);
            nowMailDetailModel.isDark = true;
        }
        else if (!nowMailDetailModel.isPhishing)
        {
        }
        else
        {
            Debug.Log("メールが見つかりません");
        }
        ShowMailList();
    }
}
