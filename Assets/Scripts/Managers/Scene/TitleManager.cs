using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject title, afterQuestionnaire, questionnaire, exitTitlePanel;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TMP_InputField nameInputText;
    [SerializeField] TMP_Dropdown age_groupD, genderD, initially_interestedD;
    private Dictionary<string, string> age_groupList = new()
    {
        {"19 歳未満","~19" },
        {"20 歳以上 30 歳未満","20~29"},
        {"30 歳以上 40 歳未満","30~39"},
        {"40 歳以上 50 歳未満","40~49"},
        {"50 歳以上 60 歳未満","50~59"},
        {"60 歳以上","60~"}
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
        Zoom(title, false);
        Zoom(questionnaire, true);
        PrepareQuestionnaire();
    }
    void ToStartScreen()
    {
        Zoom(afterQuestionnaire,true);
        nameText.text = PlayerPrefs.GetString("user_name");
    }
    void FinishQuestionnaire()
    {
        Zoom(title, true);
        Zoom(afterQuestionnaire, true);
        Zoom(questionnaire, false);
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
            Debug.Log("すべての項目に回答してください");
            return;
        }

        StartCoroutine(SendUserData(nameInputText.text,
        age_groupList[GetDropdownText(age_groupD)],
        genderList[GetDropdownText(genderD)],
        initially_interestedList[GetDropdownText(initially_interestedD)]));
    }
    private string GetDropdownText(TMP_Dropdown dropdown)
    {
        return dropdown.options[dropdown.value].text;
    }
    IEnumerator SendUserData(string user_name, string age_group, string gender, bool initially_interested)
    {
        var sendDataTask = Supabase.SendUserData(user_name, age_group, gender, initially_interested);
        yield return new WaitUntil(() => sendDataTask.IsCompleted);
        FinishQuestionnaire();
    }
}
