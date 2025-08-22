using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject battle;
    [SerializeField] TextMeshProUGUI nameText;
    public static int lastScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nameText.text = PlayerPrefs.GetString("user_name");
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
        GameDataManager.instance.ResetScore();
        SimulationAttackManager.instance.SetNextAttackTime();
        ToBattleButton();
    }
    public static void GameOver()
    {
        lastScore = GameDataManager.instance.score;
        SimulationAttackManager.instance.attackCTS.Cancel();
        SceneManager.LoadScene("ResultScene");
    }
    public void ToShopButton()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.shop;
        battle.SetActive(false);
        ShopManager.instance.EnterShop();
    }
    public void ToBattleButton()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.battle;
        ShopManager.instance.ExitShop();
        battle.SetActive(true);
    }
}
