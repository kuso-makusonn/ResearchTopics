using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class User
{
    //https://database-test.apisubdomain.workers.dev/
    private static string apiUrl = "https://database-test.apisubdomain.workers.dev/api/post/database/users";

    // 名前を送信するメソッド（呼び出しはStartやボタンなどから）
    public  static IEnumerator SendNameCoroutine(string userName)
    {
        string json = JsonUtility.ToJson(new NameData { user_name = userName });

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
                    Debug.LogError("ユーザーIDがレスポンスに含まれていません。");
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
    private class NameData
    {
        public string user_name;
    }

    [System.Serializable]
    private class UserResponse
    {
        public string user_id;
        public string user_name;
    }
}