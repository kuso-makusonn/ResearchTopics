using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField]GameObject title_image;//タイトルのイラスト
    [SerializeField]GameObject buttons;//ボタンの親
    [SerializeField]TextMeshProUGUI button1,button2;//各ボタンの文字
    public static int Change_Screen;//ボタン表示の管理
    public static bool SceneChanger=false;
    // Start is called before the first frame update
    void Start()
    {
        
        if(SceneChanger==true){
            Change_Screen=1;
            buttons.SetActive(true);
        }
        else{
            buttons.SetActive(false);
            Change_Screen=0;
        }
        
    }  

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Return)){//Enterで表示
            buttons.SetActive(true);
            Change_Screen+=1;
        }
        if(Change_Screen==0){
            buttons.SetActive(false);
        }
        if (Input.GetKey(KeyCode.Escape)){
            Escape();
        }
    }
    public void Escape(){//ボタンを消す方法
        Change_Screen=0;
    }
    public void IsButton(bool TS){
        if(TS==true){
            SceneManager.LoadScene("SimulationScene");//シミュレーションシーンに移行
        }
        if(TS==false){
            SceneManager.LoadScene("PCQuizScene");//パソコン版(仮)のクイズに移動
        }
    }
}
