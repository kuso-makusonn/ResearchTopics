using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject battle;
    [SerializeField] ShopManager shopManager;
    [SerializeField] TextMeshProUGUI nameText;
    public static int lastScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.other;
        nameText.text = PlayerPrefs.GetString("user_name");
        GameDataManager.instance.ResetScore();
        ToBattleButton();
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
    }
    async void GameStart()
    {
        await Task.Delay(1);
        // await Supabase.SendGameStart(PlayerPrefs.GetString("user_id"));
        GameDataManager.instance.screen = GameDataManager.Screen.battle;
    }
    public static void GameOver()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.other;
        lastScore = GameDataManager.instance.score;
        SceneManager.LoadScene("ResultScene");
    }
    public void ToShopButton()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.shop;
        battle.SetActive(false);
        shopManager.EnterShop();
    }
    public void ToBattleButton()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.battle;
        shopManager.ExitShop();
        battle.SetActive(true);
    }
}
