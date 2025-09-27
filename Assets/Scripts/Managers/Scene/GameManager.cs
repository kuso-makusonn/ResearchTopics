using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
    }


    [SerializeField] GameObject battle, menu, countdown;
    [SerializeField] TextMeshProUGUI nameText, countdownText;
    public int lastScore;
    public float play_time;
    private Coroutine countdownCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nameText.text = PlayerPrefs.GetString("user_name");
        StartCoroutine(GameStart());
    }

    // Update is called once per frame
    void Update()
    {
        play_time += Time.deltaTime;
    }
    IEnumerator GameStart()
    {
        yield return null;
        // if (PlayerPrefs.HasKey("user_id"))
        // {
        //     var sendDataTask = Supabase.SendGameStart(PlayerPrefs.GetString("user_id"));
        //     yield return new WaitUntil(() => sendDataTask.IsCompleted);
        // }

        GameDataManager.instance.ResetScore();
        play_time = 0f;
        EnemySpawner.instance.canSpawn = true;
        SimulationAttackManager.instance.canAttack = true;
        MailManager.instance.canSendMail = true;
        GameDataManager.instance.canVirusBusterCount = 5;
        GameDataManager.instance.screen = GameDataManager.Screen.other;
        ToBattleButton();
    }
    public void GameOver()
    {
        lastScore = GameDataManager.instance.score;
        Debug.Log(lastScore);
        ItemEffectManager.CancelAllEffect();
        SceneManager.LoadScene("ResultScene");
    }
    public void ToShopButton()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.shop;
        menu.SetActive(false);
        ShopManager.instance.EnterShop();
    }
    public void ToBattleButton()
    {
        menu.SetActive(false);
        countdownCoroutine = StartCoroutine(CountDown());
    }
    public void MenuButton()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.menu;
        menu.SetActive(true);
        if (countdownCoroutine == null) return;
        StopCoroutine(countdownCoroutine);
        countdown.SetActive(false);
    }
    public void ReturnToMenu()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.menu;
        menu.SetActive(true);
        MailManager.instance.ExitMail();
        ShopManager.instance.ExitShop();
        SimulationAttackManager.instance.ExitVirusBusterScreen();
    }
    public IEnumerator CountDown()
    {
        countdown.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);

        countdown.SetActive(false);
        GameDataManager.instance.screen = GameDataManager.Screen.battle;
        battle.SetActive(true);
    }
    public void MailButton()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.other;
        menu.SetActive(false);
        MailManager.instance.ShowMailList();
    }
    public void ToVirusBusterScreen()
    {
        GameDataManager.instance.screen = GameDataManager.Screen.other;
        menu.SetActive(false);
        SimulationAttackManager.instance.EnterVirusBusterScreen();
    }
}
