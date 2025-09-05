using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject nameInputField, nameObject, startButton, exitTitlePanel;
    [SerializeField] TMP_InputField nameInputText;
    public string age_group = "~19";
    public string gender = "none";
    public bool initially_interested = false;
    [SerializeField] TextMeshProUGUI nameText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ToTitle();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void ToTitle()
    {
        //タイトル用に画面を整理する
        exitTitlePanel.SetActive(true);
        nameInputField.SetActive(false);
        nameObject.SetActive(false);
        startButton.SetActive(false);
    }
    public void ExitTitle()
    {
        exitTitlePanel.SetActive(false);
        if (!PlayerPrefs.HasKey("user_id"))
        {
            ToInputScreen();
        }
        else
        {
            ToStartScreen();
        }
    }
    void ToInputScreen()
    {
        PlayerPrefs.DeleteAll();
        nameInputField.SetActive(true);
        nameObject.SetActive(false);
        startButton.SetActive(false);
    }
    void ToStartScreen()
    {
        nameInputField.SetActive(false);
        nameObject.SetActive(true);
        startButton.SetActive(true);
        // if (!IsPlayerPrefsExist())
        // {
        //     RefreshPlayerPrefs();
        // }
        nameText.text = PlayerPrefs.GetString("user_name");
    }
    bool IsPlayerPrefsExist()
    {
        return true;
    }
    void RefreshPlayerPrefs()
    {
    }
    public void StartButton()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void DecideNameButton()
    {
        if (string.IsNullOrWhiteSpace(nameInputText.text)) return;
        Debug.Log(nameInputText.text);
        StartCoroutine(SendUserData());
    }

    IEnumerator SendUserData()
    {
        var rank = Supabase.GetScoreRank();
        yield return new WaitUntil(() => rank.IsCompleted);
        Supabase.RankItem[] ranks = rank.Result;
        Debug.Log(ranks[0].user_name + ranks[0].score);

        var sendDataTask = Supabase.SendUserData(nameInputText.text, age_group, gender, initially_interested);
        yield return new WaitUntil(() => sendDataTask.IsCompleted);
        ToStartScreen();
    }
}
