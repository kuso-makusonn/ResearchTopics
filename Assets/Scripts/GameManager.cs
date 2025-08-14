using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject battle;
    private static GameDataManager gameDataManager;
    [SerializeField] ShopManager shopManager;
    [SerializeField] TextMeshProUGUI nameText;
    public enum Screen
    {
        battle,
        shop,
        other
    }
    public static Screen screen;
    public static int lastScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screen = Screen.other;
        gameDataManager = GetComponent<GameDataManager>();
        nameText.text = PlayerPrefs.GetString("user_name");
        gameDataManager.ResetScore();
        ToBattleButton();
        StartCoroutine(GameStart());
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator GameStart()
    {
        yield return null;
        // yield return Game.SendGameStart(PlayerPrefs.GetString("user_id"));
        screen = Screen.battle;
    }
    public static void GameOver()
    {
        screen = Screen.other;
        lastScore = gameDataManager.score;
        SceneManager.LoadScene("ResultScene");
    }
    public void ToShopButton()
    {
        screen = Screen.shop;
        battle.SetActive(false);
        shopManager.EnterShop();
    }
    public void ToBattleButton()
    {
        screen = Screen.battle;
        shopManager.ExitShop();
        battle.SetActive(true);
    }
}
