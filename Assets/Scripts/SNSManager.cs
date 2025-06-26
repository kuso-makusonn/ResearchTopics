using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class SNSManager : MonoBehaviour
{
    [SerializeField] GameObject sample, setumei, resultans, resultsco;//シミュ画面、説明文、正解不正解の表示、結果発表
    [SerializeField] TextMeshProUGUI scoreplant, resultanstxt, resultscotxt;//シミュの右上のスコア表記、正不、最終スコア
    [SerializeField] Image illustpanel;//シミュの画像の場所
    int score;//スコア用
    bool trueanswer;//各写真の正解を入れる
    public Sprite newImage1, newImage2;//仮画像の１と２
    Image image;//シミュの画像をいれる奴
    int AA = 0;//イラストの順番の仮置き
    public int MaxAA = 10;
    // Start is called before the first frame update
    void Start()
    {
        setumei.SetActive(true);
        sample.SetActive(false);
        resultans.SetActive(false);
        resultsco.SetActive(false);
        score = 0;//スコアは０点
        trueanswer = false;
        image = illustpanel.GetComponent<Image>();//イラストパネルのイラストの部分をいじれる
    }

    // Update is called once per frame
    void Update()
    {
        scoreplant.text = "スコア：" + score.ToString();
        resultscotxt.text = "スコア：" + score.ToString();
        if (Input.GetKey(KeyCode.Return))
        {
            ResultScore();//結果発表
        }
        if (Input.GetKeyDown(KeyCode.Escape)){
            TitleManager.SceneChanger=true;
            SceneManager.LoadScene("TitleScene");
        }
    }
    void SNSChecker()
    {
        if (AA < MaxAA)
        {
            if (AA % 2 == 0)
            {
                image.sprite = newImage1;
                trueanswer = false;
            }
            else if (AA % 2 == 1)
            {
                image.sprite = newImage2;
                trueanswer = true;
            }
        }
        else {
            ResultScore();//結果発表
        }
        AA++;
        resultanstxt.text = "そりゃ駄目だぜ、\nキョウダイ";
    }
    public void OpenSNS()//ボタン用(仮置き)
    {
        sample.SetActive(true);
        setumei.SetActive(false);
        SNSChecker();
    }
    public void CheckAnswer(bool Answer)//答え合わせ
    {
        if (Answer == trueanswer)
        {
            score += 100;
            //Debug.Log(score);
            resultanstxt.text = "そうだね!!";
        }
        StartCoroutine(ResultAnswer());
    }
    IEnumerator ResultAnswer()//正解・不正解の表示
    {
        resultans.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        resultans.SetActive(false);
        SNSChecker();
    }
    void ResultScore()//結果発表
    {
        ResultManager.sceneName = ResultManager.SceneName.SNSSimScene;
        ResultManager.answeredNum = MaxAA;
        ResultManager.correctNum = score / 100;
        ResultManager.score = score;
        SceneManager.LoadScene("ResultScene");
        /*resultsco.SetActive(true);
        sample.SetActive(false);
        resultans.SetActive(false);
        setumei.SetActive(false);*/
    }
}
