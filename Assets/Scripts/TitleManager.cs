using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject nameInputField, nameObject, startButton, exitTitlePanel;
    [SerializeField] TMP_InputField nameInputText;
    [SerializeField] TextMeshProUGUI nameText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ToTitle();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ToTitle()
    {
        exitTitlePanel.SetActive(true);
        nameInputField.SetActive(false);
        nameObject.SetActive(false);
        startButton.SetActive(false);
    }
    public void ExitTitle()
    {
        exitTitlePanel.SetActive(false);
        if (!PlayerPrefs.HasKey("user_id"))
        {
            ToInputScreen();
        }
        else
        {
            ToStartScreen();
        }
    }
    void ToInputScreen()
    {
        PlayerPrefs.DeleteAll();
        nameInputField.SetActive(true);
        nameObject.SetActive(false);
        startButton.SetActive(false);
    }
    void ToStartScreen()
    {
        nameInputField.SetActive(false);
        nameObject.SetActive(true);
        startButton.SetActive(true);
        if (!IsPlayerPrefsExist())
        {
            RefreshPlayerPrefs();
        }
        nameText.text = PlayerPrefs.GetString("user_name");
    }
    bool IsPlayerPrefsExist()
    {
        return true;
    }
    void RefreshPlayerPrefs()
    {
    }
    public void StartButton()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void DecideNameButton()
    {
        StartCoroutine(DecideName());
    }

    public IEnumerator DecideName()
    {
        if (string.IsNullOrWhiteSpace(nameInputText.text)
        || nameInputText.text.Length > 15)
            yield break;
        Debug.Log(nameInputText.text);
        yield return SendNameCoroutine(nameInputText.text);
        ToStartScreen();
    }
    //https://database-test.apisubdomain.workers.dev/
    private string apiUrl = "https://database-test.apisubdomain.workers.dev/api/post/database/users";

    // 名前を送信するメソッド（呼び出しはStartやボタンなどから）
    private IEnumerator SendNameCoroutine(string userName)
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
