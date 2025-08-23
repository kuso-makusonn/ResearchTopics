using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SimulationAttackManager : MonoBehaviour
{
    public static SimulationAttackManager instance;
    private List<Action> attacks = new();
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);

        // attacks.Add(Phishing);
        attacks.Add(Bot);
        attacks.Add(RansomwareStart);
    }

    [SerializeField] GameObject simulationAttacks, success, fail, countdown;
    [SerializeField] TextMeshProUGUI successDescription, failDescription, countdownText;
    public bool canAttack = false;
    private bool isAttacking = false;
    private float attackTimer;
    private float nextAttackTime;
    private GameDataManager.Screen lastScreen;

    private void Start()
    {
        SetNextAttackTime();
    }
    void Update()
    {
        if (!canAttack) return;
        if (isAttacking) return;
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
        isAttacking = false;
        simulationAttacks.SetActive(false);
    }
    private void RunRandomAttack()
    {
        isAttacking = true;
        simulationAttacks.SetActive(true);
        attacks[UnityEngine.Random.Range(0, attacks.Count)]();
    }
    private void ShowResult(bool isSuccess)
    {
        GameDataManager.instance.screen = GameDataManager.Screen.other;
        if (isSuccess)
        {
            success.SetActive(true);
        }
        else
        {
            fail.SetActive(true);
        }
    }
    public void OKButton()
    {
        success.SetActive(false);
        fail.SetActive(false);
        if (lastScreen == GameDataManager.Screen.battle)
        {
            _ = ReturnBattle();
        }
        else
        {
            GameDataManager.instance.screen = lastScreen;
            SetNextAttackTime();
        }
    }
    private async Task ReturnBattle()
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
            GameDataManager.instance.screen = lastScreen;
            SetNextAttackTime();
        }
    }
    private async Task DelayAttack(int duration, CancellationTokenSource cts)
    {
        float timer = 0;
        const int interval = 100;
        while (timer < duration)
        {
            bool last = GameDataManager.instance.screen == GameDataManager.Screen.battle;
            await Task.Delay(interval, cts.Token);
            bool now = GameDataManager.instance.screen == GameDataManager.Screen.battle;
            if (last && now)
            {
                timer += interval;
            }
        }
    }


    //
    //これより下は擬似攻撃とそれに付随するメソッド
    //

    //フィッシング
    [Header("フィッシング")]
    [SerializeField] GameObject phishing;
    private void Phishing()
    {
    }


    //ボット
    [Header("ボット")]
    [SerializeField] GameObject bot;
    CancellationTokenSource botCTS;
    private void Bot()
    {
        botCTS = new();
        _ = BotEffect(botCTS);
    }
    private async Task BotEffect(CancellationTokenSource cts)
    {
        float times = 2f;
        try
        {
            GameDataManager.instance.players[0].bulletInterval *= times;
            await DelayAttack(10000, cts);
            BotEnd(false);
        }
        finally
        {
            GameDataManager.instance.players[0].bulletInterval *= 1/times;
        }
    }
    private void BotEnd(bool isSuccess)
    {
        botCTS.Cancel();
        botCTS.Dispose();
        botCTS = null;
        lastScreen = GameDataManager.instance.screen;
        ShowResult(isSuccess);
    }
    public void VirusBaster()
    {
        if (botCTS != null)
        {
            BotEnd(true);
        }
    }

    //ランサムウェア
    [Header("ランサムウェア")]
    [SerializeField] GameObject ransomware;
    [SerializeField] TextMeshProUGUI timerText, money;
    CancellationTokenSource ransomwareTimerCTS;
    private void RansomwareStart()
    {
        lastScreen = GameDataManager.instance.screen;
        GameDataManager.instance.screen = GameDataManager.Screen.other;
        ransomware.SetActive(true);
        ransomwareTimerCTS = new();
        _ = TimerStart(ransomwareTimerCTS);
    }
    private void RansomwareEnd(bool isSuccess)
    {
        ransomwareTimerCTS.Cancel();
        ransomwareTimerCTS.Dispose();
        ransomwareTimerCTS = null;
        ransomware.SetActive(false);
        ShowResult(isSuccess);
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
        RansomwareEnd(false);
    }
    private async Task TimerStart(CancellationTokenSource cts)
    {
        int seconds = 60;
        timerText.text = TimerText(seconds);
        while (seconds > 0)
        {
            await Task.Delay(1000, cts.Token);
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