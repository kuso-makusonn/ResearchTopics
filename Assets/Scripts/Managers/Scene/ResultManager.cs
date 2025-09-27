using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject gameOver, result, rankRootObject, rankArea, rankPrefab;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI nameText;
    List<RankManager> rankManagers = new();
    string game_id;
    bool rankIn;
    int index;
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
        if (PlayerPrefs.HasKey("now_game_id")
        && GameManager.instance != null)
        {
            game_id = PlayerPrefs.GetString("now_game_id");

            var sendDataTask = Supabase.SendGameResult(PlayerPrefs.GetString("now_game_id"),
            GameManager.instance.lastScore,
            GameManager.instance.play_time);
            yield return new WaitUntil(() => sendDataTask.IsCompleted);
        }

        PlayerPrefs.DeleteKey("now_game_id");
        yield return GameOver();
        yield return ShowResult();
        yield return ShowRank();
        SceneManager.LoadScene("TitleScene");
    }
    private void ClearScreen()
    {
        gameOver.SetActive(false);
        result.SetActive(false);
        rankRootObject.SetActive(false);
    }
    IEnumerator GameOver()
    {
        ClearScreen();
        gameOver.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);//3000ミリ秒待つ
    }
    IEnumerator ShowResult()
    {
        ClearScreen();
        result.SetActive(true);
        if (GameManager.instance.lastScore >= 0)
        {
            scoreText.text = "Score:" + GameManager.instance.lastScore.ToString();
        }
        yield return new WaitForSecondsRealtime(3f); ;//3000ミリ秒待つ
    }
    IEnumerator ShowRank()
    {
        var rankRes = Supabase.GetScoreRank();
        yield return new WaitUntil(() => rankRes.IsCompleted);
        Supabase.RankItem[] ranks = rankRes.Result;

        ClearScreen();
        rankRootObject.SetActive(true);

        for (int i = rankArea.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(rankArea.transform.GetChild(i).gameObject);
        }
        rankManagers = new();
        for (int i = 0; i < 5; i++)
        {
            RankManager rankManager = Instantiate(rankPrefab, rankArea.transform).GetComponent<RankManager>();
            rankManager.Highlight(false);
            rankManagers.Add(rankManager);
            if (i + 1 <= ranks.Length)
            {
                rankManager.Init(i + 1, ranks[i].user_name, ranks[i].score);
            }
            else
            {
                rankManager.Null(i + 1);
            }
        }
        rankIn = false;
        for (int i = 0; i < 5; i++)
        {
            if (i + 1 <= ranks.Length
            && ranks[i].game_id == game_id)
            {
                rankIn = true;
                index = i;
            }
        }
        if (rankIn)
        {
            RankManager rankManager = rankManagers[index];
            for (int i = 0; i < 5; i++)
            {
                rankManager.Highlight(true);
                yield return new WaitForSeconds(0.5f);
                rankManager.Highlight(false);
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            yield return new WaitForSeconds(5f);
        }
    }
}
