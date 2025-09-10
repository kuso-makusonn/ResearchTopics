using TMPro;
using UnityEngine;

public class MailController : MonoBehaviour
{
    [SerializeField] GameObject dark;
    [SerializeField] TextMeshProUGUI titleText, senderText, mainText;
    public MailModel mailModel;
    public void Init(MailModel _mailModel)
    {
        mailModel = _mailModel;
        ShowMail();
    }
    public void ShowMail()
    {
        titleText.text = mailModel.title;
        senderText.text = $"From:{mailModel.sender}";
        mainText.text = mailModel.main;
        dark.SetActive(mailModel.isDark ? true : false);
    }
    public void SelectMailButton()
    {
        MailManager.instance.ShowMailDetail(mailModel);
    }
}
