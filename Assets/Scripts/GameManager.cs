using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //public static bool Mail=false;
    public static bool Mail=true;
    public static bool Web = false;
    [SerializeField]GameObject mail, web;
    [SerializeField]GameObject AnsPanel1;
    [SerializeField]TextMeshProUGUI mailText;
    [SerializeField]TextMeshProUGUI testText;
    TextAsset csvFile;// CSVファイル
    int q = 1;
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
    // Start is called before the first frame update
    void Start()
    {
        if(Mail==true){
            mail.SetActive(true);
            csvFile = Resources.Load("TestMail") as TextAsset; // Resouces下のCSV読み込み
            StringReader reader = new StringReader(csvFile.text);
            // , で分割しつつ一行ずつ読み込み
            // リストに追加していく
            while (reader.Peek() != -1) // reader.Peaekが-1になるまで
            {
                string line = reader.ReadLine(); // 一行ずつ読み込み
                csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
            }
            TextChange();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)){
            TitleManager.SceneChanger=true;
            SceneManager.LoadScene("TitleScene");
            Mail=false;
        }
    }
    void TextChange(){
        mailText.text=csvDatas[q][1].Replace("\\n","\n");
    }
}
