using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

//データベース登録用クラス
public static class Supabase
{
    private static string rootURL = "https://researchtopics-database-api.apisubdomain.workers.dev";
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

    public static async Task SendUserData(string user_name, string age_group, string gender, bool initially_interested)
    {
        string address = "/api/post/database/users";
        UserData gameData = new(user_name, age_group, gender, initially_interested);
        string postJson = JsonUtility.ToJson(gameData);
        string resJson = await PostJson(address, postJson);
        if (resJson == null) return;
        UserResponse[] user = JsonHelper.FromJson<UserResponse>(resJson);
        PlayerPrefs.SetString("user_id", user[0].user_id);
        PlayerPrefs.SetString("user_name", user_name);
        PlayerPrefs.SetString("age_group", age_group);
        PlayerPrefs.SetString("gender", gender);
        PlayerPrefs.SetString("initially_interested", initially_interested.ToString());
        PlayerPrefs.Save(); // 明示的に保存（省略可）
        Debug.Log("ユーザーデータ保存完了");
    }
    // JSONに変換するためのクラス
    [System.Serializable]
    private class UserData
    {
        public string user_name;
        public string age_group;
        public string gender;
        public bool initially_interested;
        public UserData(string _user_name, string _age_group, string _gender, bool _initially_interested)
        {
            user_name = _user_name;
            age_group = _age_group;
            gender = _gender;
            initially_interested = _initially_interested;
        }
    }

    [System.Serializable]
    private class UserResponse
    {
        public string user_id;
    }

    public static async Task SendGameStart(string user_id)
    {
        string address = "/api/post/database/games";
        GameData gameData = new(user_id);
        string postJson = JsonUtility.ToJson(gameData);
        string resJson = await PostJson(address, postJson);
        if (resJson == null) return;
        GameResponse[] game = JsonHelper.FromJson<GameResponse>(resJson);
        PlayerPrefs.SetString("now_game_id", game[0].game_id);
        PlayerPrefs.Save(); // 明示的に保存（省略可）
        Debug.Log("ゲームID保存完了");
    }
    // JSONに変換するためのクラス
    [System.Serializable]
    private class GameData
    {
        public string user_id;
        public GameData(string _user_id)
        {
            user_id = _user_id;
        }
    }

    [System.Serializable]
    private class GameResponse
    {
        public string game_id;
    }

    public static async Task SendGameResult(string game_id, int score, float play_time)
    {
        string address = "/api/post/database/scores";
        ResultData gameData = new(game_id, score, play_time);
        string postJson = JsonUtility.ToJson(gameData);
        string resJson = await PostJson(address, postJson);
        if (resJson == null) return;
        ResultResponse[] gameResult = JsonHelper.FromJson<ResultResponse>(resJson);
        if (!PlayerPrefs.HasKey("high_score") || gameResult[0].score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("high_score", gameResult[0].score);
            PlayerPrefs.Save(); // 明示的に保存（省略可）
            Debug.Log("ハイスコア更新！");
        }
    }

    // JSONに変換するためのクラス
    [System.Serializable]
    private class ResultData
    {
        public string game_id;
        public int score;
        public float play_time;
        public ResultData(string _game_id, int _score, float _play_time)
        {
            game_id = _game_id;
            score = _score;
            play_time = _play_time;
        }
    }

    [System.Serializable]
    private class ResultResponse
    {
        public int score;
    }

    public static async Task SendAttackLog(string game_id, string attack_id, bool handled, float response_time)
    {
        string address = "/api/post/database/attacks";
        AttackLog gameData = new(game_id, attack_id, handled, response_time);
        string postJson = JsonUtility.ToJson(gameData);
        _ = await PostJson(address, postJson);
    }
    [System.Serializable]
    private class AttackLog
    {
        public string game_id;
        public string attack_id;
        public bool handled;
        public float response_time;
        public AttackLog(string _game_id, string _attack_id, bool _handled, float _response_time)
        {
            game_id = _game_id;
            attack_id = _attack_id;
            handled = _handled;
            response_time = _response_time;
        }
    }

    public static async Task<RankItem[]> GetScoreRank()
    {
        string address = "/api/get/database/rank";
        using (UnityWebRequest request = UnityWebRequest.Get(rootURL + address))
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield(); // 非同期で待つ

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + request.error);
                return null;
            }

            string json = request.downloadHandler.text;
            return JsonHelper.FromJson<RankItem>(json);
        }
    }
    [System.Serializable]
    public class RankItem
    {
        public string user_name;
        public string game_id;
        public int score;
    }
}