using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject title_image;//タイトルのイラスト
    [SerializeField] GameObject buttons;//ボタンの親
    [SerializeField] GameObject statusPanel;
    [SerializeField] TextMeshProUGUI totalScoreText, quizHighScoreText, mailHighScoreText, webHighScoreText, SNSHighScoreText, SSIDHighScoreText;
    public static bool SceneChanger = false;
    bool isTitle = true;
    Dictionary<ResultManager.SceneName, TextMeshProUGUI> scoreTexts;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneChanger == true)
        {
            SceneChanger = false;
            ToTitleAndButtons();
        }
        else
        {
            ToOnlyTitle();
        }
        scoreTexts = new Dictionary<ResultManager.SceneName, TextMeshProUGUI>()
        {
            {ResultManager.SceneName.QuizScene, quizHighScoreText},
            {ResultManager.SceneName.MailSimScene, mailHighScoreText},
            {ResultManager.SceneName.WebSimScene, webHighScoreText},
            {ResultManager.SceneName.SNSSimScene, SNSHighScoreText},
            {ResultManager.SceneName.SSIDSimScene, SSIDHighScoreText},
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (
        isTitle
        && ((Input.touchCount > 0
                && Input.GetTouch(0).phase == TouchPhase.Began)
            || Input.GetMouseButtonDown(0)
            || Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCode.Escape))
        )
        {
            //タッチ出力
            ToTitleAndButtons();
        }
        else
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToOnlyTitle();
        }
    }
    void ToTitleAndButtons()
    {
        isTitle = false;
        buttons.SetActive(true);
        statusPanel.SetActive(false);
    }
    void ToOnlyTitle()
    {
        isTitle = true;
        buttons.SetActive(false);
        statusPanel.SetActive(false);
    }
    public void ToOtherSceneButton(bool TS)
    {
        if (TS == true)
        {
            SceneManager.LoadScene("SelectScene");//シミュレーションシーンに移行
        }
        if (TS == false)
        {
            SceneManager.LoadScene("PCQuizScene");//パソコン版(仮)のクイズに移動
        }
    }
    public void ShowStatus(bool doShow)
    {
        if (doShow)
        {
            int score;
            int total = 0;
            foreach (ResultManager.SceneName name in System.Enum.GetValues(typeof(ResultManager.SceneName)))
            {
                score = PlayerPrefs.GetInt($"HighScore_{name}", 0);
                scoreTexts[name].text = score.ToString();
                total += score;
            }
            totalScoreText.text = total.ToString();
            statusPanel.SetActive(true);
        }
        else
        {
            statusPanel.SetActive(false);
        }
    }
}
