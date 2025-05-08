using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebSimSceneManager : MonoBehaviour
{
    [SerializeField] GameObject webSimStartPanel,rootWebSim,answerPanel;
    [SerializeField] RectTransform contentSize;
    [SerializeField] Image webPage;//ページ画像のコンポーネント
    [SerializeField] Sprite webSprite;
    float webWidth = 1900;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WebSimStart());
    }
    IEnumerator WebSimStart(){
        rootWebSim.SetActive(false);
        answerPanel.SetActive(false);
        webSimStartPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        webSimStartPanel.SetActive(false);
        SetWebPage();
    }
    void SetWebPage(){
        webPage.sprite = webSprite;
        contentSize.sizeDelta = new Vector2(webWidth,webWidth*(webSprite.rect.height/webSprite.rect.width) + 600);
        rootWebSim.SetActive(true);
    }
    public void ToAnswerPanelButton(){
        answerPanel.SetActive(true);
    }
    public void ReturnWebPanelButton(){
        answerPanel.SetActive(false);
    }
    public void CorrectButton(){
    }
    public void NotCorrectButton(){
    }
}
