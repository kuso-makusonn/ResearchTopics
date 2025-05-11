using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WebSimSceneManager : MonoBehaviour
{
    [SerializeField] GameObject webSimStartPanel,questionNumberPanel,rootWebSim,answerPanel,resultPanel;
    [SerializeField] RectTransform contentTransform;
    [SerializeField] TextMeshProUGUI questionNumberText,resultText;
    [SerializeField] Image webPage;//ページ画像のコンポーネント
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip correctSound,notCorrectSound;
    Sprite webSprite;
    List<Sprite> safeSprits,notSafeSprits;
    const float webWidth = 1900;
    int qNum,score;
    bool isSafe,answer;
    // Start is called before the first frame update
    void Start()
    {
        qNum = 1;
        score = 0;
        StartCoroutine(WebSimStart());
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
    IEnumerator WebSimStart(){
        ClearScreen();
        if(qNum == 1){
            safeSprits = Resources.LoadAll<Sprite>("WebPage/Safe").ToList();
            notSafeSprits = Resources.LoadAll<Sprite>("WebPage/NotSafe").ToList();
            webSimStartPanel.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            webSimStartPanel.SetActive(false);
        }
        if(safeSprits.Count > 0 && notSafeSprits.Count > 0){
            questionNumberText.text = qNum + "問目！";
            questionNumberPanel.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            questionNumberPanel.SetActive(false);
            Debug.Log("ウェブページセット準備");
            if(Random.Range(0,2) == 0){
                Debug.Log("安全な方");
                isSafe = true;
                webSprite = safeSprits[Random.Range(0,safeSprits.Count)];
                safeSprits.Remove(webSprite);
            }else{
                Debug.Log("危険な方");
                isSafe = false;
                webSprite = notSafeSprits[Random.Range(0,notSafeSprits.Count)];
                notSafeSprits.Remove(webSprite);
            }
            webPage.sprite = webSprite;
            contentTransform.sizeDelta = new Vector2(webWidth,200 + webWidth*(webSprite.rect.height/webSprite.rect.width) + 400);
            contentTransform.position = new Vector2(0,0);
            rootWebSim.SetActive(true);
            Debug.Log("ウェブページセット完了！");
        }else{
            Debug.Log("データもうねえよ!");
            if(qNum > 1){
                resultPanel.SetActive(true);
                resultText.text = "終了！";
                yield return new WaitForSeconds(1.5f);
                resultText.text = $"{score}問正解！\n正答率{(int)((float)score/(qNum-1)*100)}%";
                yield return new WaitForSeconds(3f);
                resultPanel.SetActive(false);
                SceneManager.LoadScene("SelectScene");
            }else{
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
        answerPanel.SetActive(false);
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
