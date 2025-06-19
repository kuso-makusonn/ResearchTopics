using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public enum SceneName//シーンリスト
    {
        QuizScene,
        MailSimScene,
        SNSSimScene,
        SSIDSimScene,
        WebSimScene,
    };
    public static SceneName sceneName;//どのシーンからなのか上のリストから選ぶ
    public static int answeredNum;//”既に答えた”問題数
    public static int correctNum;//↑の内、正解した問題数
    public static int score;//スコア
    static Dictionary<SceneName, int> maxScoreDict = new Dictionary<SceneName, int>();//Dictionaryでシーンとスコアを関連付けて保持
    static bool isInit = false;//伊豆井ニット
    // Start is called before the first frame update
    void Start()
    {
        if (!isInit)
        {
            foreach (SceneName sceneName in System.Enum.GetValues(typeof(SceneName)))
            {
                maxScoreDict.Add(sceneName, 0); // 初期スコアは0
            }
            isInit = true;
        }
        if (score > maxScoreDict[sceneName])
        {
            maxScoreDict[sceneName] = score;
        }
        Debug.Log(sceneName+answeredNum.ToString()+correctNum.ToString()+score.ToString());
    }

    // Update is called once per frame
    void Update()
    {
    }
}
