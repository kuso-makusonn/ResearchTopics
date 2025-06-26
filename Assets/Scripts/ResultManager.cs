using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [ContextMenu("Reset High Scores")]
    public void ResetHighScores()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs をリセットしました");
    }
    public enum SceneName//シーンリスト
    {
        QuizScene,
        MailSimScene,
        WebSimScene,
        SNSSimScene,
        SSIDSimScene,
    };
    public static SceneName sceneName;//どのシーンからなのか上のリストから選ぶ
    public static int answeredNum;//”既に答えた”問題数
    public static int correctNum;//↑の内、正解した問題数
    public static int score;//スコア
    [SerializeField] GameObject scorePanel, statusPanel, quizHighScoreTextObject, mailHighScoreTextObject, webHighScoreTextObject, SNSHighScoreTextObject, SSIDHighScoreTextObject;
    [SerializeField] TextMeshProUGUI scoreText, totalScoreText, quizHighScoreText, mailHighScoreText, webHighScoreText, SNSHighScoreText, SSIDHighScoreText;
    Dictionary<SceneName, int> highScoreDict = new Dictionary<SceneName, int>();//Dictionaryでシーンとスコアを関連付けて保持
    Dictionary<SceneName, TextMeshProUGUI> scoreTexts;
    Dictionary<SceneName, GameObject> highScoreTextObjects;
    // bool isOrigin = true;

    void Start()
    {
        // if (isOrigin)
        // {
        //     OriginInit();
        // }
        InitDictionaries();
        ShowHighScoreTexts();
        StartCoroutine(ShowResult());
    }
    // void OriginInit()
    // {
    //     PlayerPrefs.DeleteAll();
    //     PlayerPrefs.Save();
    //     isOrigin = false;
    // }
    void InitDictionaries()
    {
        scoreTexts = new Dictionary<SceneName, TextMeshProUGUI>()
        {
            {SceneName.QuizScene, quizHighScoreText},
            {SceneName.MailSimScene, mailHighScoreText},
            {SceneName.WebSimScene, webHighScoreText},
            {SceneName.SNSSimScene, SNSHighScoreText},
            {SceneName.SSIDSimScene, SSIDHighScoreText},
        };
        highScoreTextObjects = new Dictionary<SceneName, GameObject>()
        {
            {SceneName.QuizScene, quizHighScoreTextObject},
            {SceneName.MailSimScene, mailHighScoreTextObject},
            {SceneName.WebSimScene, webHighScoreTextObject},
            {SceneName.SNSSimScene, SNSHighScoreTextObject},
            {SceneName.SSIDSimScene, SSIDHighScoreTextObject},
        };
        ImportHighScore();
    }
    void ShowHighScoreTexts()
    {
        foreach (SceneName name in System.Enum.GetValues(typeof(SceneName)))
        {
            scoreTexts[name].text = highScoreDict[name].ToString();
            highScoreTextObjects[name].SetActive(false);
        }
        totalScoreText.text = highScoreDict.Values.Sum().ToString();
    }
    void ImportHighScore()
    {
        foreach (SceneName name in System.Enum.GetValues(typeof(SceneName)))
        {
            highScoreDict.TryAdd(name, PlayerPrefs.GetInt($"HighScore_{name}", 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TitleManager.SceneChanger = true;
            SceneManager.LoadScene("TitleScene");
        }
    }
    IEnumerator ShowResult()
    {
        scorePanel.SetActive(false);
        statusPanel.SetActive(false);

        scoreText.text = "スコア：" + score.ToString();
        scorePanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        scorePanel.SetActive(false);
        statusPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (score > highScoreDict[sceneName])
        {
            for (int i = 0; i < 3; i++)
            {
                ShowHighScoreHighlight(true);
                yield return new WaitForSeconds(0.5f);

                ShowHighScoreHighlight(false);
                yield return new WaitForSeconds(0.5f);
            }
            ShowHighScoreHighlight(true);
            yield return new WaitForSeconds(1f);

            highScoreDict[sceneName] = score;
            PlayerPrefs.SetInt($"HighScore_{sceneName}", score);
            PlayerPrefs.Save();
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }
        statusPanel.SetActive(false);
        TitleManager.SceneChanger = true;
        SceneManager.LoadScene("TitleScene");
    }
    void ShowHighScoreHighlight(bool isHighLight)
    {
        int current = isHighLight ? score : highScoreDict[sceneName];
        Color color = isHighLight ? Color.yellow : Color.white;

        scoreTexts[sceneName].text = current.ToString();
        scoreTexts[sceneName].color = color;

        totalScoreText.text = isHighLight ? (highScoreDict.Values.Sum() + score - highScoreDict[sceneName]).ToString() : highScoreDict.Values.Sum().ToString();
        totalScoreText.color = color;

        highScoreTextObjects[sceneName].SetActive(isHighLight);
    }
}
