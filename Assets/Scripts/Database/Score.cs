using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class Score
{
        //https://database-test.apisubdomain.workers.dev/
    public static string apiUrl = "https://database-test.apisubdomain.workers.dev/api/post/database/scores";
    public static IEnumerator SendGameResult(string gameId, int score)
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