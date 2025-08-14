using System.Collections;
using TMPro;
using UnityEngine;
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
        yield return User.SendNameCoroutine(nameInputText.text);
        ToStartScreen();
    }
}
