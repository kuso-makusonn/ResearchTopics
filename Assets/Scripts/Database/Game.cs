using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class Game
{
    //https://database-test.apisubdomain.workers.dev/
    private static string apiUrl = "https://database-test.apisubdomain.workers.dev/api/post/database/games";
    public static IEnumerator SendGameStart(string userId)
    {
        string json = JsonUtility.ToJson(new GameData { user_id = userId });

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

                GameResponse game = JsonUtility.FromJson<GameResponse>(resJson);

                if (game.user_id == userId)
                {
                    PlayerPrefs.SetString("game_id", game.game_id);
                    PlayerPrefs.Save(); // 明示的に保存（省略可）

                    Debug.Log("ゲーム開始データ保存完了");
                }
                else
                {
                    Debug.LogError("ゲームIDがレスポンスに含まれていません。");
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
        public string user_id;
    }

    [System.Serializable]
    private class GameResponse
    {
        public string game_id;
        public string user_id;
    }
}
