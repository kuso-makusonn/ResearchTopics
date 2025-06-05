using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SSIDSimManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI b1Text, b2Text, b3Text, b4Text, ansText, scoreText;//テキスト
    [SerializeField] GameObject title, quiz, quizStartPanel, ansPanel, difficultySelection, dangerPanel;
    Image dangerPanelImage;
    TextAsset csvFile;// CSVファイル
    List<string[]> csvData = new List<string[]>(); // CSVの中身を入れるリスト;
    int difficulty;//難易度番号1~4　数字が小さいほうが簡単
    int Ans = 0;//回答番号
    int q = 1;//問題番号
    int score = 0, wrongNum = 0;
    const int maxWrong = 7;//n回間違えるとでゲームオーバー
    public AudioClip correctSound, notCorrectSound;//音1,音2
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        dangerPanelImage = dangerPanel.GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();//音のComponentを取得
        csvFile = Resources.Load("課題研究-SSID") as TextAsset; // Resources下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);
        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvData.Add(line.Split(',')); // , 区切りでリストに追加
        }
        ClearScreen();
        title.SetActive(true);
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            TitleManager.SceneChanger = true;
            SceneManager.LoadScene("TitleScene");
        }
    }
    void ClearScreen()
    {
        quiz.SetActive(false);
        quizStartPanel.SetActive(false);
        ansPanel.SetActive(false);
        difficultySelection.SetActive(false);
    }
    public void SSIDSimStart()
    {
        title.SetActive(false);
        difficultySelection.SetActive(true);//難易度選択画面表示
    }
    void QASet()
    {
        //問いの文章と各選択肢を設定
        ClearScreen();
        quiz.SetActive(true);
        b1Text.text = csvData[q][0];
        b2Text.text = csvData[q][1];
        b3Text.text = csvData[q][2];
        b4Text.text = csvData[q][3];
    }

    //回答ボタン関数
    public void A()
    {
        Ans = 1;//ボタン1を押した反応
        CheckAnswer();
    }
    public void B()
    {
        Ans = 2;//ボタン1を押した反応
        CheckAnswer();
    }
    public void C()
    {
        Ans = 3;//ボタン1を押した反応
        CheckAnswer();
    }
    public void D()
    {
        Ans = 4;//ボタン1を押した反応
        CheckAnswer();
    }
    public void CheckAnswer()
    {
        //正誤判定
        if (Ans == int.Parse(csvData[q][4]))
        {
            ansText.text = "正解だよ";
            audioSource.PlayOneShot(correctSound);//音１を鳴らす
            score += 100;
            wrongNum--;
            if (wrongNum < 0)
            {
                wrongNum = 0;
            }
        }
        else
        {
            ansText.text = "違うよ";
            audioSource.PlayOneShot(notCorrectSound);//音２を鳴らす
            wrongNum++;
            if (wrongNum == 1)
            {
                StartCoroutine(ToDanger());
            }
            if (wrongNum >= maxWrong)
            {
                ClearScreen();
                Debug.Log("GameOver");
            }
        }
        ShowScore();
        StartCoroutine(ShowAnsPanels());
    }
    IEnumerator ShowAnsPanels()
    {
        //正解不正解を表示
        ansPanel.SetActive(true);//パネルを表示
        yield return new WaitForSeconds(2f);
        ansPanel.SetActive(false);//パネルを非表示
        if (wrongNum < maxWrong)
        {
            q++;//カウントを増やす
            QASet();//クイズ・アンサーを設定
        }
    }

    /// <summary>
    /// 難易度選択ボタンの関数A~D
    /// difficultyに難易度1~4 数字が小さいほうが簡単
    /// ToQAで問題へ移行
    /// </summary>
    public void DfcButtonA()
    {
        difficulty = 1;
        StartCoroutine(QuizStart());
    }
    public void DfcButtonB()
    {
        difficulty = 2;
        StartCoroutine(QuizStart());
    }
    public void DfcButtonC()
    {
        difficulty = 3;
        StartCoroutine(QuizStart());
    }
    public void DfcButtonD()
    {
        difficulty = 4;
        StartCoroutine(QuizStart());
    }
    IEnumerator QuizStart()
    {
        ClearScreen();
        quizStartPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        quizStartPanel.SetActive(false);
        quiz.SetActive(true);
        score = 0;
        q = 1;
        wrongNum = 0;
        ShowScore();
        QASet();//クイズ読み込み・表示
    }
    void ShowScore()
    {
        scoreText.text = $"Score:{score}";
    }
    IEnumerator ToDanger()
    {
        bool isIncreasing = true;
        const float defaultR = 255f;
        const float maxA = 100f;
        const int changeNum = 4;//n回目から点滅開始
        float rRate;
        dangerPanel.SetActive(true);
        while (wrongNum > 0)
        {
            if (wrongNum < changeNum)
            {
                dangerPanelImage.color = new Color(defaultR / 255f, dangerPanelImage.color.g, dangerPanelImage.color.b, maxA / (changeNum - 1) * wrongNum / 255f);
            }
            else
            {
                rRate = 2f * (wrongNum - (changeNum - 1));
                if (isIncreasing)
                {
                    dangerPanelImage.color = new Color(dangerPanelImage.color.r + rRate / 255f, dangerPanelImage.color.g, dangerPanelImage.color.b, maxA / 255f);
                    if (dangerPanelImage.color.r >= 1)
                    {
                        isIncreasing = false;
                    }
                }
                else
                {
                    dangerPanelImage.color = new Color(dangerPanelImage.color.r - rRate / 255f, dangerPanelImage.color.g, dangerPanelImage.color.b, maxA / 255f);
                    if (dangerPanelImage.color.r <= 0)
                    {
                        isIncreasing = true;
                    }
                }
            }
            yield return null;
        }
        dangerPanel.SetActive(false);
    }
}