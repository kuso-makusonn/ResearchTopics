using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimulationAttackManager : MonoBehaviour
{
    public static SimulationAttackManager instance;
    private class SimulationAttack
    {
        public SimulationAttack(Action _action, string _successDescription, string _failDescription)
        {
            action = _action;
            successDescription = _successDescription;
            failDescription = _failDescription;
        }
        public Action action;
        public string successDescription;
        public string failDescription;
    }
    private List<SimulationAttack> attacks = new();
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);

        attacks.Add(new(Phishing, phishingSuccessDescription, phishingFailDescription));
        attacks.Add(new(Bot, botSuccessDescription, botFailDescription));
        attacks.Add(new(Ransomware, ransomwareSuccessDescription, ransomwareFailDescription));
    }
    public enum State
    {
        notAttacking,
        phishing,
        bot,
        ransomware
    }
    public State nowState = State.notAttacking;
    private Dictionary<string, string> attack_id = new()
    {
        {"phishing","ph_v1.0.0"},
        {"bot","bo_v1.0.0"},
        {"ransomware","ra_v1.0.0"}
    };

    [SerializeField] GameObject simulationAttacks, simulationAttacksZoom, simulationAttacksNotZoom, success, fail, countdown, reward, penalty, rewardItems, penaltyItem;
    [SerializeField] TextMeshProUGUI successDescription, failDescription, countdownText;
    public bool canAttack = false;
    private float attackTimer;
    private float nextAttackTime;
    private GameDataManager.Screen lastScreen;
    private bool isResponseTime = false;
    public float response_time;
    private int nowAttackIndex;

    private void Start()
    {
        SetNextAttackTime();
    }
    void Update()
    {
        if (!canAttack) return;
        if (nowState != State.notAttacking)
        {
            if (isResponseTime)
            {
                response_time += Time.deltaTime;
            }
            return;
        }
        if (GameDataManager.instance.screen != GameDataManager.Screen.battle) return;

        // ゲームが動いている間のみカウントアップ
        attackTimer += Time.deltaTime;

        if (attackTimer >= nextAttackTime)
        {
            RunRandomAttack();
        }
    }
    private void SetNextAttackTime()
    {
        nextAttackTime = UnityEngine.Random.Range(10, 20);
        attackTimer = 0f;
        nowState = State.notAttacking;
        isResponseTime = false;
        // simulationAttacks.SetActive(false);
        // StartCoroutine(SetActiveExtension.Zoom(simulationAttacksZoom, false));
        // simulationAttacksNotZoom.SetActive(false);
    }
    private void RunRandomAttack()
    {
        // simulationAttacks.SetActive(true);
        // StartCoroutine(SetActiveExtension.Zoom(simulationAttacksZoom, true));
        // simulationAttacksNotZoom.SetActive(true);
        nowAttackIndex = UnityEngine.Random.Range(0, attacks.Count - 1);
        attacks[nowAttackIndex].action();
        Debug.Log(attacks[nowAttackIndex].successDescription);
    }
    public void ShowResult(bool isSuccess, int descriptionIndex = -1)
    {
        GameDataManager.instance.screen = GameDataManager.Screen.other;

        // 擬似攻撃ログデータ送信
        if (!(descriptionIndex >= 0 && descriptionIndex < attacks.Count)
        && GameManager.instance.now_game_id != null
        && nowState != State.notAttacking
        && isResponseTime
        && canAttack)
        {
            _ = Supabase.SendAttackLog(GameManager.instance.now_game_id,
            attack_id[nowState.ToString()],
            isSuccess,
            response_time);
        }

        if (!(descriptionIndex >= 0 && descriptionIndex < attacks.Count))
        {
            if (isSuccess)
            {
                successDescription.text = attacks[nowAttackIndex].successDescription;
                StartCoroutine(SetActiveExtension.Zoom(success, true));
            }
            else
            {
                failDescription.text = attacks[nowAttackIndex].failDescription;
                StartCoroutine(SetActiveExtension.Zoom(fail, true));
            }
        }
        else
        {
            if (isSuccess)
            {
                successDescription.text = attacks[descriptionIndex].successDescription;
                StartCoroutine(SetActiveExtension.Zoom(success, true));
            }
            else
            {
                failDescription.text = attacks[descriptionIndex].failDescription;
                StartCoroutine(SetActiveExtension.Zoom(fail, true));
            }
        }
    }
    public void SuccessOkButton()
    {
        StartCoroutine(SetActiveExtension.Zoom(success, false));
        Reward();
    }
    void Reward()
    {
        StartCoroutine(SetActiveExtension.Zoom(reward, true));
        ShopManager.instance.CreateRewardItems(rewardItems);
    }
    public void FailOkButton()
    {
        StartCoroutine(SetActiveExtension.Zoom(fail, false));
        Penalty();
    }
    void Penalty()
    {
        StartCoroutine(SetActiveExtension.Zoom(penalty, true));
        ShopManager.instance.CreatePenaltyItem(penaltyItem);
    }
    public void ExitSimulationAttackResultScreen(bool isSuccess)
    {
        StartCoroutine(ExitSimulationAttackResultScreenIE(isSuccess));
    }
    IEnumerator ExitSimulationAttackResultScreenIE(bool isSuccess)
    {
        if (isSuccess)
        {
            yield return SetActiveExtension.Zoom(reward, false);
        }
        else
        {
            yield return SetActiveExtension.Zoom(penalty, false);
        }
        if (nowState == State.notAttacking)
        {
        }
        else
        {
            if (lastScreen == GameDataManager.Screen.battle)
            {
                countdownCoroutine = StartCoroutine(ReturnBattle());
            }
            else
            {
                GameDataManager.instance.screen = lastScreen;
                SetNextAttackTime();
            }
        }
    }
    private Coroutine countdownCoroutine;
    private IEnumerator ReturnBattle()
    {
        countdown.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        countdown.SetActive(false);
        GameDataManager.instance.screen = GameDataManager.Screen.battle;
        SetNextAttackTime();
    }
    public void ToMenuWhenCountDown()
    {
        StopCountDown();
        countdown.SetActive(false);
        GameManager.instance.MenuButton();
    }
    private void StopCountDown()
    {
        StopCoroutine(countdownCoroutine);
        SetNextAttackTime();
    }
    private IEnumerator WaitAttack(float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            bool last = GameDataManager.instance.screen == GameDataManager.Screen.battle;
            yield return null;
            bool now = GameDataManager.instance.screen == GameDataManager.Screen.battle;
            if (last && now)
            {
                timer += Time.deltaTime;
            }
        }
    }
    public void StartResponseTime()
    {
        response_time = 0f;
        isResponseTime = true;
    }


    //
    //これより下は擬似攻撃とそれに付随するメソッド
    //

    //フィッシング
    [Header("フィッシング")]
    [SerializeField] GameObject phishing;
    //成功失敗時の文章変えるのはこういうとこにあるよ
    private string phishingSuccessDescription =
    "フィッシング詐欺はSNSからも狙ってきます。メールだけでなくSNSの利用時も警戒心を持ち、情報が正しいか確認する習慣をつけましょう。";
    private string phishingFailDescription =
    "フィッシング詐欺はなりすましで情報を盗みます。急かすメールのリンクはクリックせず、情報を入力する前に本物のサイトか必ず確認しましょう。";
    private void Phishing()
    {
        nowState = State.phishing;
        MailManager.instance.isPhishingMailAttacking = true;
    }
    public void PhishingEnd(bool isSuccess)
    {
        lastScreen = GameDataManager.instance.screen;
        ShowResult(isSuccess);
    }


    //ボット
    [Header("ボット")]
    [SerializeField] GameObject bot, virusBusterScreenZoom, virusBusterScreenNotZoom, OiOiOiPanel;
    private Coroutine botEffectCoroutine;
    [SerializeField] TextMeshProUGUI canVirusBusterCountText;
    private string botSuccessDescription =
    "動作が遅い以外にもボット攻撃のサインはありますが、それに頼るだけでなく、常に多様な方法で本人確認を行い、アカウントを強固に守りましょう。";
    private string botFailDescription =
    "動作が遅いのはボット攻撃のサインかもしれません。アカウントを守るために、いくつかの方法で本人確認をしましょう。";
    private void Bot()
    {
        nowState = State.bot;
        botEffectCoroutine = StartCoroutine(BotEffect());
    }
    private IEnumerator BotEffect()
    {
        StartResponseTime();
        GameDataManager.instance.players[0].bulletInterval *= 10f;
        yield return WaitAttack(10);
        BotEnd(false);
    }
    private void BotEnd(bool isSuccess)
    {
        StopCoroutine(botEffectCoroutine);
        botEffectCoroutine = null;
        GameDataManager.instance.players[0].bulletInterval /= 10f;
        lastScreen = GameDataManager.instance.screen;
        ShowResult(isSuccess);
    }
    public void EnterVirusBusterScreen()
    {
        StartCoroutine(SetActiveExtension.Zoom(virusBusterScreenZoom, true));
        virusBusterScreenNotZoom.SetActive(true);
        canVirusBusterCountText.text = $"残り可能回数：{GameDataManager.instance.canVirusBusterCount}";
    }
    public void ExitVirusBusterScreen()
    {
        StartCoroutine(SetActiveExtension.Zoom(virusBusterScreenZoom, false));
        virusBusterScreenNotZoom.SetActive(false);
    }
    public void VirusBuster()
    {
        if (GameDataManager.instance.canVirusBusterCount < 1)
        {
            StartCoroutine(ShowOiOiOiPanel());
            return;
        }
        if (botEffectCoroutine != null)
        {
            BotEnd(true);
        }
        else
        {
            ShowResult(false, 1);
        }
        GameDataManager.instance.canVirusBusterCount--;
        canVirusBusterCountText.text = $"残り可能回数：{GameDataManager.instance.canVirusBusterCount}";
    }
    IEnumerator ShowOiOiOiPanel()
    {
        OiOiOiPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        OiOiOiPanel.SetActive(false);
    }

    //ランサムウェア
    [Header("ランサムウェア")]
    [SerializeField] GameObject ransomware;
    [SerializeField] TextMeshProUGUI timerText, money, checkText;
    [SerializeField] GameObject checkPanel, payButton, shutDownButton, wifiButton;
    private Coroutine timerCoroutine;
    private string ransomwareSuccessDescription =
    "ランサムウェアは、USB接続からも感染する可能性があります。簡単に復旧できるバック  アップを常に行い、日々の利用で警戒を怠らないようにしましょう。";
    private string ransomwareFailDescription =
    "ランサムウェアはファイルを人質にとってお金を要求する攻撃です。データのバックアップと怪しいファイルを開かないことが最大の防御になります。";
    private void Ransomware()
    {
        nowState = State.ransomware;
        StartResponseTime();
        lastScreen = GameDataManager.instance.screen;
        GameDataManager.instance.screen = GameDataManager.Screen.other;
        StartCoroutine(SetActiveExtension.Zoom(ransomware, true));
        timerCoroutine = StartCoroutine(TimerStart());
    }
    private void RansomwareEnd(bool isSuccess)
    {
        checkPanel.SetActive(false);
        StopCoroutine(timerCoroutine);
        StartCoroutine(SetActiveExtension.Zoom(ransomware, false));
        ShowResult(isSuccess);
    }
    public void CheckP()
    {
        checkPanel.SetActive(true);
        payButton.SetActive(true);
        wifiButton.SetActive(false);
        shutDownButton.SetActive(false);
        checkText.text = "本当に支払いますか？";
    }
    public void CheckW()
    {
        checkPanel.SetActive(true);
        payButton.SetActive(false);
        wifiButton.SetActive(true);
        shutDownButton.SetActive(false);
        checkText.text = "ネット接続を切りますか？";
    }
    public void CheckT()
    {
        checkPanel.SetActive(true);
        payButton.SetActive(false);
        wifiButton.SetActive(false);
        shutDownButton.SetActive(true);
        checkText.text = "電源を落としますか？";
    }
    public void CloseCheckPanel()
    {
        checkPanel.SetActive(false);
    }
    public void Pay()
    {
        Debug.Log("払っちゃうんだ！？");
        GameDataManager.instance.MoneyUp(-10000);
        RansomwareEnd(false);
    }
    public void WiFi()
    {
        Debug.Log("ネット接続を切ります");
        RansomwareEnd(true);
    }
    public void TurnOff()
    {
        Debug.Log("電源を落とします");
        PlayerManager.StopMove = true;
        RansomwareEnd(false);
    }
    private IEnumerator TimerStart()
    {
        int seconds = 60;
        timerText.text = TimerText(seconds);
        while (seconds > 0)
        {
            yield return new WaitForSeconds(1f);
            seconds--;
            timerText.text = TimerText(seconds);
        }
        RansomwareEnd(false);
    }
    private string TimerText(int seconds)
    {
        return $"{seconds / 60:D2}:{seconds % 60:D2}";
    }
}