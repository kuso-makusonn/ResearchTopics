using System.Collections;
using System.Threading.Tasks;
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
        YMD();
    }

    // Update is called once per frame
    void Update()
    {
    }
    async void YMD()
    {
        // await Supabase.SendGameResult(PlayerPrefs.GetString("game_id"), GameManager.lastScore);
        await GameOver();
        await ShowResult();
        SceneManager.LoadScene("TitleScene");
    }
    async Task GameOver()
    {
        gameOver.SetActive(true);
        result.SetActive(false);
        await Task.Delay(3000);//3000ミリ秒待つ
    }
    async Task ShowResult()
    {
        result.SetActive(true);
        gameOver.SetActive(false);
        scoreText.text = "Score:" + GameManager.lastScore.ToString();
        await Task.Delay(3000);//3000ミリ秒待つ
    }
}
