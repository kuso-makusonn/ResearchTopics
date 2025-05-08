using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]GameObject mail,mailTextPanel,truePanal;
    [SerializeField]GameObject AnsPanel1;
    [SerializeField]TextMeshProUGUI mailText,trueText,startText;
    [SerializeField]GameObject mailStartPanel;
    TextAsset csvFile;// CSVファイル
    bool TruePanals;
    int q = 1;
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MailStart());
        //mail.SetActive(true);
        csvFile = Resources.Load("TestMail") as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);
        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)){
            TitleManager.SceneChanger=true;
            SceneManager.LoadScene("TitleScene");
        }
    }
    void TextChange(){
        mailText.text=csvDatas[q][1].Replace("\\n","\n");
        trueText.text="アドレス\n"+csvDatas[q][2]+"\nURL\n"+csvDatas[q][3];
    }
    IEnumerator MailStart(){
        mailStartPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        mailStartPanel.SetActive(false);
        AnsPanel1.SetActive(true);
        PoPMove.Reseter=false;
        TextChange();
        TruePanals = true;
    }
    public void MailPanelOn(){
        AnsPanel1.SetActive(false);
        mail.SetActive(true);
        mailTextPanel.SetActive(true);
    }
    public void onAnsPanelS(){
        AnsPanel1.SetActive(true);
        mail.SetActive(false);
    }
    public void onTruePanelS(){
        truePanal.SetActive(TruePanals);
        TruePanals = !TruePanals;
    }
    public void AnswerMail(string answer){
        if(answer==csvDatas[q][4]){
            startText.text="そのとーり";
        }
        else{
            startText.text="それは違うぜ";
        }
        q+=1;
        StartCoroutine(MailStart());
    }
}
