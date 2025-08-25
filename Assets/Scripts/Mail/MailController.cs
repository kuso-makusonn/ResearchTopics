using TMPro;
using UnityEngine;

public class MailController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText, senderText, mainText;
    public MailEntity mailEntity;
    public void Init(MailEntity _mailEntity)
    {
        mailEntity = _mailEntity;
        ShowMail();
    }
    public void ShowMail()
    {
        if (mailEntity == null) return;
        titleText.text = mailEntity.title;
        senderText.text = $"From:{mailEntity.sender}";
        mainText.text = mailEntity.main;
    }
    public void SelectMailButton()
    {
        MailManager.instance.ShowMailDetail(mailEntity);
    }
}
