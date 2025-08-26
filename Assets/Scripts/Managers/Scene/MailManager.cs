using System.Collections.Generic;
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


    [SerializeField] GameObject mail, mailListObj, mailArea, mailPrefab, sendedMail, mailDetail;
    [SerializeField] ScrollRect mailListScrollRect;
    [SerializeField] RectTransform mailDetailContent, mailDetailBackground, title, sender, main, linkButton, link;
    [SerializeField] TextMeshProUGUI titleText, senderText, mainText, linkText;
    public List<MailEntity> mailList = new();
    public MailEntity[] allMailEntities;
    public MailEntity[] allPhishingMailEntities;
    public bool canSendMail;
    private bool isSendingMail;
    private float sendMailTimer;
    private float nextSendMailTime;
    public bool isPhishingMailAttacking;

    private void Start()
    {
        allMailEntities = Resources.LoadAll<MailEntity>("Mails/MailEntities");
        allPhishingMailEntities = Resources.LoadAll<MailEntity>("Mails/PhishingMailEntities");
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
        sendedMail.SetActive(false);

        mailListObj.SetActive(true);
        mailDetail.SetActive(false);
        mail.SetActive(true);
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
        mail.SetActive(false);
    }
    private void NewMail(int index)
    {
        if (index < 0 || index > allMailEntities.Length - 1)
        {
            Debug.Log("そんなメールは無えょ！");
            return;
        }
        mailList.Add(allMailEntities[index]);
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
        mailList.Add(allPhishingMailEntities[index]);
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
    }
    public void ShowMailDetail(MailEntity mailEntity)
    {
        mailListObj.SetActive(false);
        mailDetail.SetActive(true);
        titleText.text = mailEntity.title;
        senderText.text = $"From:{mailEntity.sender}";
        mainText.text = mailEntity.main;
        linkText.text = mailEntity.link;

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
    }
    public void WarningButton()
    {
    }
}
