using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject battle, menu, countdown;
    [SerializeField] TextMeshProUGUI nameText, countdownText;
    public static int lastScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nameText.text = PlayerPrefs.GetString("user_name");
        StartCoroutine(GameStart());
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator GameStart()
    {
        yield return null;
        // var sendDataTask = Supabase.SendGameStart(PlayerPrefs.GetString("user_id"));
        // yield return new WaitUntil(() => sendDataTask.IsCompleted);
        GameDataManager.instance.ResetScore();
        SimulationAttackManager.instance.canAttack = true;
        GameDataManager.instance.screen = GameDataManager.Screen.other;
        ToBattleButton();
    }
    public static void GameOver()
    {
        lastScore = GameDataManager.instance.score;
        SceneManager.LoadScene("ResultScene");
    }
    public void ToShopButton()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.shop;
        menu.SetActive(false);
        ShopManager.instance.EnterShop();
    }
    public async void ToBattleButton()
    {
        menu.SetActive(false);
        await CountDown();
        GameDataManager.instance.screen = GameDataManager.Screen.battle;
        battle.SetActive(true);
    }
    public void MenuButton()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.menu;
        menu.SetActive(true);
    }
    public void ReturnToMenu()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.menu;
        menu.SetActive(true);
        ShopManager.instance.ExitShop();
    }
    public async Task CountDown()
    {
        try
        {
            countdown.SetActive(true);
            for (int i = 3; i > 0; i--)
            {
                countdownText.text = i.ToString();
                await Task.Delay(1000);
            }
            countdownText.text = "GO!";
            await Task.Delay(1000);
        }
        finally
        {
            countdown.SetActive(false);
        }
    }
}
