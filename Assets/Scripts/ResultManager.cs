using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
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
        yield return SendGameResult(PlayerPrefs.GetString("game_id"), GameManager.lastScore);
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
    //https://database-test.apisubdomain.workers.dev/
    private string apiUrl = "https://database-test.apisubdomain.workers.dev/api/post/database/scores";
    IEnumerator SendGameResult(string gameId, int score)
    {
        string json = JsonUtility.ToJson(new GameData { game_id = gameId, score = score});

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 10;

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("送信成功: " + request.downloadHandler.text);
                // レスポンスのJSONを配列として扱う
                string resJson = request.downloadHandler.text;

                // // 配列で返ってくるので、[]を含めてパースする
                // UserResponse[] users = JsonHelper.FromJson<UserResponse>(resJson);

                GameResponse gameResult = JsonUtility.FromJson<GameResponse>(resJson);

                if (gameResult.game_id == gameId
                && gameResult.score == score)
                {
                    PlayerPrefs.SetInt("score", gameResult.score);
                    PlayerPrefs.Save(); // 明示的に保存（省略可）

                    Debug.Log("リザルトデータ保存完了");
                }
                else
                {
                    Debug.LogError("レスポンスが正しくありません");
                }
            }
            else
            {
                Debug.LogError($"送信失敗: {request.responseCode} | {request.error} | {request.downloadHandler.text}");
            }
        }
    }

    // JSONに変換するためのクラス
    [System.Serializable]
    private class GameData
    {
        public string game_id;
        public int score;
    }

    [System.Serializable]
    private class GameResponse
    {
        public string game_id;
        public int score;
    }
}
