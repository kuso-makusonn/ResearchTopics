using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject gameover, result;
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
        // yield return Score.SendGameResult(PlayerPrefs.GetString("game_id"), GameManager.lastScore);
        yield return GameOver();
        yield return ShowResult();
        SceneManager.LoadScene("TitleScene");
    }
    IEnumerator GameOver()
    {
        gameover.SetActive(true);
        result.SetActive(false);
        yield return new WaitForSeconds(3f);
        yield return ShowResult();
    }
    IEnumerator ShowResult()
    {
        result.SetActive(true);
        gameover.SetActive(false);
        scoreText.text = "Score:" + GameManager.lastScore.ToString();
        yield return new WaitForSeconds(3f);
    }
}
