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
    [SerializeField]TextMeshProUGUI quiz,b1,b2,b3,b4,at;//テキスト
    [SerializeField] GameObject anspanel;
    TextAsset csvFile;// CSVファイル
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    int Ans=0;
    int q = 1;
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
        QASet();
    }
    void Update(){
        if (Input.GetKey(KeyCode.Escape)){
            TitleManager.Change_Screen=0;
            SceneManager.LoadScene("TitleScene");
        }
    }

    void QASet(){//問いの文章と各選択肢を設定
        quiz.text=csvDatas[q][1];
        b1.text=csvDatas[q][2];
        b2.text=csvDatas[q][3];
        b3.text=csvDatas[q][4];
        b4.text=csvDatas[q][5];
    }
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
            at.text="正解だよ";
            audioSource.PlayOneShot(sound1);//音１を鳴らす
        }
        else{
            at.text="違うよ";
            audioSource.PlayOneShot(sound2);//音２を鳴らす
        }
        StartCoroutine(AnsPanels());
    }
    IEnumerator AnsPanels(){//正解不正解を表示
        anspanel.SetActive(true);//パネルを表示
        yield return new WaitForSeconds(2f);
        anspanel.SetActive(false);//パネルを非表示
        q++;//カウントを増やす
        QASet();//クイズ・アンサーを設定
    }
}