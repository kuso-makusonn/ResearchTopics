using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject title, afterQuestionnaire, questionnaire, exitTitlePanel, surveyConsent, OiOiPanel;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TMP_InputField nameInputText;
    [SerializeField] TMP_Dropdown age_groupD, genderD, initially_interestedD;
    private Dictionary<string, string> age_groupList = new()
    {
        {"19歳未満","~19" },
        {"20歳以上 30歳未満","20~29"},
        {"30歳以上 40歳未満","30~39"},
        {"40歳以上 50歳未満","40~49"},
        {"50歳以上 60歳未満","50~59"},
        {"60歳以上","60~" },
        {"無回答","none"}
    };
    private Dictionary<string, string> genderList = new()
    {
        {"男性","male"},
        {"女性","female"},
        {"無回答","none"}
    };
    private Dictionary<string, bool> initially_interestedList = new()
    {
        {"はい",true},
        {"いいえ",false}
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ToTitle();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void Zoom(GameObject gameObject, bool toActive, float duration = 0.5f)
    {
        StartCoroutine(SetActiveExtension.Zoom(gameObject, toActive, duration));
    }
    void ToTitle()
    {
        //タイトル用に画面を整理する
        title.SetActive(true);
        exitTitlePanel.SetActive(true);
        afterQuestionnaire.SetActive(false);
        questionnaire.SetActive(false);
        surveyConsent.SetActive(false);
    }
    public void ExitTitle()
    {
        exitTitlePanel.SetActive(false);
        if (!PlayerPrefs.HasKey("user_id"))
        {
            ToQuestionnaireScreen();
        }
        else
        {
            ToStartScreen();
        }
    }
    void ToQuestionnaireScreen()
    {
        PlayerPrefs.DeleteAll();
        PrepareQuestionnaire();
        // Zoom(questionnaire, true);
        Zoom(surveyConsent, true);
    }
    void ToStartScreen()
    {
        Zoom(afterQuestionnaire, true);
        nameText.text = PlayerPrefs.GetString("user_name");
    }
    void FinishSendingQuestionnaireData()
    {
        Zoom(afterQuestionnaire, true);
        nameText.text = PlayerPrefs.GetString("user_name");
    }
    public void StartButton()
    {
        SceneManager.LoadScene("GameScene");
    }
    private void PrepareQuestionnaire()
    {
        nameInputText.text = "";
        PrepareOptions(age_groupD, new(age_groupList.Keys));
        PrepareOptions(genderD, new(genderList.Keys));
        PrepareOptions(initially_interestedD, new(initially_interestedList.Keys));
    }
    private void PrepareOptions(TMP_Dropdown dropdown, List<string> options)
    {
        dropdown.ClearOptions();
        List<string> notSelectedOption = new()
        {
            "未選択"
        };
        dropdown.AddOptions(notSelectedOption);
        dropdown.AddOptions(options);
        dropdown.value = 0;
    }
    public void ConsentCanceled()
    {
        exitTitlePanel.SetActive(true);
        Zoom(title, true);
        Zoom(surveyConsent, false);
    }
    public void ConsentAccepted()
    {
        Zoom(surveyConsent, false);
        Zoom(questionnaire, true);
    }
    private bool IsAnswerEnough()
    {
        if (string.IsNullOrWhiteSpace(nameInputText.text)) return false;
        if (age_groupD.value == 0) return false;
        if (genderD.value == 0) return false;
        if (initially_interestedD.value == 0) return false;
        return true;
    }
    public void DecideButton()
    {
        if (!IsAnswerEnough())
        {
            StartCoroutine(OiOi());
            return;
        }
        StartCoroutine(SendUserData(nameInputText.text,
        age_groupList[GetDropdownText(age_groupD)],
        genderList[GetDropdownText(genderD)],
        initially_interestedList[GetDropdownText(initially_interestedD)]));

        exitTitlePanel.SetActive(false);
        title.SetActive(true);
        questionnaire.SetActive(false);
    }
    private string GetDropdownText(TMP_Dropdown dropdown)
    {
        return dropdown.options[dropdown.value].text;
    }
    IEnumerator SendUserData(string user_name, string age_group, string gender, bool initially_interested)
    {
        var sendDataTask = Supabase.SendUserData(user_name, age_group, gender, initially_interested);
        yield return new WaitUntil(() => sendDataTask.IsCompleted);
        FinishSendingQuestionnaireData();
    }
    private IEnumerator OiOi()
    {
        OiOiPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        OiOiPanel.SetActive(false);
    }
}
