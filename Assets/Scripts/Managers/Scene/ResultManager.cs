using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject gameOver, result;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI nameText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nameText.text = PlayerPrefs.GetString("user_name");
        StartCoroutine(YMD());
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator YMD()
    {
        // var sendDataTask =  Supabase.SendGameResult(PlayerPrefs.GetString("game_id"), GameManager.lastScore);
        // yield return new WaitUntil(() => sendDataTask.IsCompleted);
        yield return GameOver();
        yield return ShowResult();
        SceneManager.LoadScene("TitleScene");
    }
    IEnumerator GameOver()
    {
        gameOver.SetActive(true);
        result.SetActive(false);
        yield return new WaitForSecondsRealtime(3f);//3000ミリ秒待つ
    }
    IEnumerator ShowResult()
    {
        result.SetActive(true);
        gameOver.SetActive(false);
        scoreText.text = "Score:" + GameManager.lastScore.ToString();
        yield return new WaitForSecondsRealtime(3f);;//3000ミリ秒待つ
    }
}
