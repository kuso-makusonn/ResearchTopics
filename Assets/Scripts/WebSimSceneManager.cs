using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WebSimSceneManager : MonoBehaviour
{
    [SerializeField] GameObject title,webSimStartPanel,questionNumberPanel,rootWebSim,answerPanel,resultPanel;
    [SerializeField] RectTransform contentTransform;
    [SerializeField] TextMeshProUGUI questionNumberText,resultText;
    [SerializeField] Image webPage;//ページ画像のコンポーネント
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip correctSound,notCorrectSound;
    string webLink;
    const string rootWebLink = "https://sites.google.com/view/2025-meiden-j3a-j06/";
    const int safeLinkCount = 1,notSafeLinkCount = 1;
    List<string> safeLinks,notSafeLinks;
    // Sprite webSprite;
    // List<Sprite> safeSprits,notSafeSprits;
    // const float webWidth = 1900;
    int qNum,score;
    bool isSafe,answer;
    const int maxQuizNum = 10;
    // Start is called before the first frame update
    void Start()
    {
        qNum = 1;
        score = 0;
        title.SetActive(true);
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            TitleManager.SceneChanger=true;
            SceneManager.LoadScene("TitleScene");
        }
    }
    void ClearScreen(){
        webSimStartPanel.SetActive(false);
        rootWebSim.SetActive(false);
        questionNumberPanel.SetActive(false);
        answerPanel.SetActive(false);
        resultPanel.SetActive(false);
    }
    public void WebSimStartButton()
    {
        title.SetActive(false);
        StartCoroutine(WebSimStart());
    }
    IEnumerator WebSimStart(){
        ClearScreen();
        //yield return null;
        if (qNum == 1)
        {
            safeLinks = new List<string>();
            notSafeLinks = new List<string>();
            for (int i = 1; i <= safeLinkCount; i++)
            {
                safeLinks.Add(rootWebLink + "safe/" + i.ToString());
            }
            for (int i = 1; i <= notSafeLinkCount; i++)
            {
                notSafeLinks.Add(rootWebLink + "notsafe/" + i.ToString());
            }
            // safeSprits = Resources.LoadAll<Sprite>("WebPage/Safe").ToList();
            // notSafeSprits = Resources.LoadAll<Sprite>("WebPage/NotSafe").ToList();
            webSimStartPanel.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            webSimStartPanel.SetActive(false);
        }
        if(safeLinks.Count > 0 && notSafeLinks.Count > 0 && qNum <= maxQuizNum){
            questionNumberText.text = qNum + "問目！";
            questionNumberPanel.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            questionNumberPanel.SetActive(false);
            Debug.Log("ウェブページセット準備");
            if(Random.Range(0,2) == 0){
                Debug.Log("安全な方");
                isSafe = true;
                webLink = safeLinks[Random.Range(0,safeLinks.Count)];
                safeLinks.Remove(webLink);
                // webSprite = safeSprits[Random.Range(0,safeSprits.Count)];
                // safeSprits.Remove(webSprite);
            }else{
                Debug.Log("危険な方");
                isSafe = false;
                webLink = notSafeLinks[Random.Range(0,notSafeLinks.Count)];
                notSafeLinks.Remove(webLink);
                // webSprite = notSafeSprits[Random.Range(0,notSafeSprits.Count)];
                // notSafeSprits.Remove(webSprite);
            }
            // Application.OpenURL(webLink);
            Debug.Log(webLink);
            // webPage.sprite = webSprite;
            // contentTransform.sizeDelta = new Vector2(webWidth,200 + webWidth*(webSprite.rect.height/webSprite.rect.width) + 400);
            // contentTransform.position = new Vector2(0,0);

            answerPanel.SetActive(true);
            // rootWebSim.SetActive(true);
            Debug.Log("ウェブページセット完了！");
        }else{
            Debug.Log("データもうねえよ!");
            if (qNum > 1)
            {
                // resultPanel.SetActive(true);
                // resultText.text = "終了！";
                // yield return new WaitForSeconds(1.5f);
                // resultText.text = $"{score}問正解！\n正答率{(int)((float)score/(qNum-1)*100)}%";
                // yield return new WaitForSeconds(3f);
                // resultPanel.SetActive(false);
                // SceneManager.LoadScene("SelectScene");
                ResultManager.sceneName = ResultManager.SceneName.WebSimScene;
                ResultManager.answeredNum = qNum - 1;
                ResultManager.correctNum = score;
                ResultManager.score = score * 100;
                SceneManager.LoadScene("ResultScene");
            }
            else
            {
                Debug.Log("まだ一問もできてねぇよ！");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;  // プレイモード終了
#endif
            }
        }
    }
    public void ToAnswerPanelButton(){
        answerPanel.SetActive(true);
    }
    public void ReturnWebPanelButton(){
        Application.OpenURL(webLink);
        // answerPanel.SetActive(false);
    }
    public void SafeButton(){
        answer = true;
        StartCoroutine(CheckAnswer());
    }
    public void NotSafeButton(){
        answer = false;
        StartCoroutine(CheckAnswer());
    }
    IEnumerator CheckAnswer(){//政党判定
        resultPanel.SetActive(true);
        if(answer == isSafe){
            score++;
            resultText.text="正解だよ";
            audioSource.PlayOneShot(correctSound);//音１を鳴らす
        }
        else{
            resultText.text="違うよ";
            audioSource.PlayOneShot(notCorrectSound);//音２を鳴らす
        }
        yield return new WaitForSeconds(1.5f);
        resultPanel.SetActive(false);
        qNum++;
        StartCoroutine(WebSimStart());
    }
}
