using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

//データベース登録用クラス
public static class Supabase
{
    private static string rootURL = "https://database-test.apisubdomain.workers.dev";
    private static async Task<string> PostJson(string address, string json)
    {
        using (UnityWebRequest request = new UnityWebRequest(rootURL + address, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 10;

            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield(); // フレームをまたいで待機

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("送信成功: " + request.downloadHandler.text);
                // レスポンスのJSONを配列として扱う
                string resJson = request.downloadHandler.text;

                // 配列で返ってくるので、[]を含めてパースする
                // UserResponse[] users = JsonHelper.FromJson<UserResponse>(resJson);

                return resJson;
            }
            else
            {
                Debug.LogError($"送信失敗: {request.responseCode} | {request.error} | {request.downloadHandler.text}");
                return null;
            }
        }
    }

    public static async Task SendUserData(string userName)
    {
        string address = "/api/post/database/users";
        UserData gameData = new UserData { user_name = userName };
        string postJson = JsonUtility.ToJson(gameData);
        string resJson = await PostJson(address, postJson);
        if (resJson == null) return;
        UserResponse user = JsonUtility.FromJson<UserResponse>(resJson);

        if (user.user_name == userName)
        {
            PlayerPrefs.SetString("user_name", user.user_name);
            PlayerPrefs.SetString("user_id", user.user_id);
            PlayerPrefs.Save(); // 明示的に保存（省略可）

            Debug.Log("ユーザーデータ保存完了");
        }
        else
        {
            Debug.LogError("レスポンスが正しくありません");
        }
    }
    // JSONに変換するためのクラス
    [System.Serializable]
    private class UserData
    {
        public string user_name;
    }

    [System.Serializable]
    private class UserResponse
    {
        public string user_id;
        public string user_name;
    }

    public static async Task SendGameStart(string userId)
    {
        string address = "/api/post/database/games";
        GameData gameData = new GameData { user_id = userId };
        string postJson = JsonUtility.ToJson(gameData);
        string resJson = await PostJson(address, postJson);
        if (resJson == null) return;
        GameResponse game = JsonUtility.FromJson<GameResponse>(resJson);

        if (game.user_id == userId)
        {
            PlayerPrefs.SetString("game_id", game.game_id);
            PlayerPrefs.Save(); // 明示的に保存（省略可）

            Debug.Log("ゲーム開始データ保存完了");
        }
        else
        {
            Debug.LogError("レスポンスが正しくありません");
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

    public static async Task SendGameResult(string gameId, int score)
    {
        string address = "/api/post/database/scores";
        ResultData gameData = new ResultData { game_id = gameId };
        string postJson = JsonUtility.ToJson(gameData);
        string resJson = await PostJson(address, postJson);
        if (resJson == null) return;
        ResultResponse gameResult = JsonUtility.FromJson<ResultResponse>(resJson);

        if (gameResult.game_id == gameId
        && gameResult.score == score)
        {
            // PlayerPrefs.SetInt("score", gameResult.score);
            // PlayerPrefs.Save(); // 明示的に保存（省略可）

            Debug.Log("リザルトデータ保存完了");
        }
        else
        {
            Debug.LogError("レスポンスが正しくありません");
        }
    }

    // JSONに変換するためのクラス
    [System.Serializable]
    private class ResultData
    {
        public string game_id;
        public int score;
    }

    [System.Serializable]
    private class ResultResponse
    {
        public string game_id;
        public int score;
    }
}
