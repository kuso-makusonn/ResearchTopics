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
    [SerializeField] TextMeshProUGUI titleText, senderText, mainText;
    [SerializeField] RectTransform mailDetailRectTransform;
    List<MailEntity> mailList = new();
    public MailEntity[] allMailEntities;
    public bool canSendMail;
    private bool isSendingMail;
    private float sendMailTimer;
    private float nextSendMailTime;

    private void Start()
    {
        allMailEntities = Resources.LoadAll<MailEntity>("Mails/MailEntities");
        SetNextSendMailTime();
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
            SendNewMail();
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
            RectTransform rect = mailArea.transform as RectTransform;
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            rect.anchoredPosition = new Vector2(
                rect.anchoredPosition.x,
                -rect.rect.height / 2
            );
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
    public void DeleteMail(int index)
    {
        if (index < 0 || index > mailList.Count - 1)
        {
            Debug.Log("そんなメールは無えょ！");
            return;
        }
        mailList.RemoveAt(index);
    }
    private void SendNewMail()
    {
        sendedMail.SetActive(true);
        isSendingMail = true;
        NewMail(UnityEngine.Random.Range(0, mailList.Count - 1));
    }
    public void ShowMailDetail(MailEntity mailEntity)
    {
        titleText.text = mailEntity.title;
        senderText.text = $"From:{mailEntity.sender}";
        mainText.text = mailEntity.main;

        RectTransform rect = mailDetailRectTransform;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        rect.anchoredPosition = new Vector2(
            rect.anchoredPosition.x,
            -rect.rect.height / 2
        );

        mailListObj.SetActive(false);
        mailDetail.SetActive(true);
    }
}
