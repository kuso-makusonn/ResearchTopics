using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI quizText,b1Text,b2Text,b3Text,b4Text,ansText;//テキスト
    [SerializeField] GameObject rootQuiz,quiz,quizStartPanel,ansPanel,difficultySelection;
    TextAsset csvFile;// CSVファイル
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    int difficulty;//難易度番号1~4　数字が小さいほうが簡単
    int Ans=0;//回答番号
    int q = 1;//問題番号
    public AudioClip sound1,sound2;//音1,音2
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();//音のComponentを取得
        csvFile = Resources.Load("課題研究-クイズ") as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);
        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
        difficultySelection.SetActive(true);//難易度選択画面表示
        rootQuiz.SetActive(false);//クイズ画面隠す
    }
    void Update(){
        if (Input.GetKey(KeyCode.Escape)){
            TitleManager.SceneChanger=true;
            SceneManager.LoadScene("TitleScene");
        }
    }

    void QASet(){//問いの文章と各選択肢を設定
        ansPanel.SetActive(false);
        quizText.text=csvDatas[q][1];
        b1Text.text=csvDatas[q][2];
        b2Text.text=csvDatas[q][3];
        b3Text.text=csvDatas[q][4];
        b4Text.text=csvDatas[q][5];
    }

    //回答ボタン関数
    public void A(){
        Ans=1;//ボタン1を押した反応
        CheckAnswer();
    }
    public void B(){
        Ans=2;//ボタン1を押した反応
        CheckAnswer();
    }
    public void C(){
        Ans=3;//ボタン1を押した反応
        CheckAnswer();
    }
    public void D(){
        Ans=4;//ボタン1を押した反応
        CheckAnswer();
    }
    public void CheckAnswer(){//政党判定
        if(Ans==int.Parse(csvDatas[q][6])){
            ansText.text="正解だよ";
            audioSource.PlayOneShot(sound1);//音１を鳴らす
        }
        else{
            ansText.text="違うよ";
            audioSource.PlayOneShot(sound2);//音２を鳴らす
        }
        StartCoroutine(AnsPanels());
    }
    IEnumerator AnsPanels(){//正解不正解を表示
        ansPanel.SetActive(true);//パネルを表示
        yield return new WaitForSeconds(2f);
        ansPanel.SetActive(false);//パネルを非表示
        q++;//カウントを増やす
        QASet();//クイズ・アンサーを設定
    }

    /// <summary>
    /// 難易度選択ボタンの関数A~D
    /// difficultyに難易度1~4 数字が小さいほうが簡単
    /// ToQAで問題へ移行
    /// </summary>
    public void DfcButtonA(){
        difficulty = 1;
        ToQA();
    }
    public void DfcButtonB(){
        difficulty = 2;
        ToQA();
    }
    public void DfcButtonC(){
        difficulty = 3;
        ToQA();
    }
    public void DfcButtonD(){
        difficulty = 4;
        ToQA();
    }
    void ToQA(){
        rootQuiz.SetActive(true);//クイズアクティブ
        quiz.SetActive(false);//クイズ画面は隠す（スタート画面出すため）
        difficultySelection.SetActive(false);//難易度選択画面隠す
        StartCoroutine(QuizStart());
    }
    IEnumerator QuizStart(){
        quizStartPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        quizStartPanel.SetActive(false);
        quiz.SetActive(true);
        QASet();//クイズ読み込み・表示
    }
}