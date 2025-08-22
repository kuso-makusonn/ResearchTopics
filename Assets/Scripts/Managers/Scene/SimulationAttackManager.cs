using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SimulationAttackManager : MonoBehaviour
{
    public static SimulationAttackManager instance;
    private List<Func<Task>> attacks = new List<Func<Task>>();
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);

        // attacks.Add(Phishing);
        // attacks.Add(Bot);
        attacks.Add(Ransomware);
    }

    [SerializeField] GameObject simulationAttacks, success, fail;
    [SerializeField] TextMeshProUGUI successDescription, failDescription;
    public CancellationTokenSource attackCTS = new();
    private bool isSuccess;
    private GameDataManager.Screen lastScreen;

    private void OnDestroy()
    {
        if (attackCTS != null && !attackCTS.IsCancellationRequested)
        {
            attackCTS.Cancel();
        }
    }
    public async void SetNextAttackTime()
    {
        // simulationAttacks.SetActive(false);
        int nextAttackTime = UnityEngine.Random.Range(MS(1.5f), MS(2f));
        await RunRandomAttack(nextAttackTime);
    }
    private int MS(float minutes)
    {
        return (int)(minutes * 60 * 1000);
    }
    private async Task RunRandomAttack(int waitMS)
    {
        attackCTS = new();
        try
        {
            Debug.Log("Wait a minute...");

            // await DelayAttack(waitMS);
            await DelayAttack(10000, GameDataManager.Screen.battle);

            Debug.Log("Attack!");
            GameDataManager.instance.isAttacking = true;
            isSuccess = false;
            await attacks[UnityEngine.Random.Range(0, attacks.Count)]();
            ShowResult();
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Attack was canceled");
            // SetNextAttackTime();
        }
        finally
        {
            Debug.Log("Finish!");
            GameDataManager.instance.isAttacking = false;
            attackCTS.Dispose();
        }
    }
    private void ShowResult()
    {
        lastScreen = GameDataManager.instance.screen;
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
        GameDataManager.instance.screen = lastScreen;
        SetNextAttackTime();
    }
    private async Task DelayAttack(int delay, GameDataManager.Screen waitScreen)
    {
        int elapsed = 0;
        const int interval = 100;
        while (elapsed < delay)
        {
            await Task.Delay(interval, attackCTS.Token);
            if (GameDataManager.instance.screen == waitScreen)
            {
                elapsed += interval;
            }
        }
    }


    //
    //これより下は擬似攻撃とそれに付随するメソッド
    //

    //フィッシング
    [Header("フィッシング")]
    [SerializeField] GameObject phishing;
    private async Task Phishing()
    {
    }


    //ボット
    [Header("ボット")]
    [SerializeField] GameObject bot;
    private async Task Bot()
    {
    }

    //ランサムウェア
    [Header("ランサムウェア")]
    [SerializeField] GameObject ransomware;
    [SerializeField] TextMeshProUGUI timer, money;
    private bool ransomwareEnded;
    private async Task Ransomware()
    {
        lastScreen = GameDataManager.instance.screen;
        ransomwareEnded = false;
        try
        {
            GameDataManager.instance.screen = GameDataManager.Screen.other;
            ransomware.SetActive(true);
            await TimerStart();
        }
        finally
        {
            ransomware.SetActive(false);
            GameDataManager.instance.screen = lastScreen;
        }
    }
    public void Pay()
    {
        Debug.Log("払っちゃうんだ！？");
        isSuccess = false;
        ransomwareEnded = true;
    }
    public void WiFi()
    {
        Debug.Log("ネット接続を切ります");
        isSuccess = true;
        ransomwareEnded = true;
    }
    public void TurnOff()
    {
        Debug.Log("電源を落とします");
        isSuccess = false;
        ransomwareEnded = true;
    }
    private async Task TimerStart()
    {
        int seconds = 60;
        timer.text = TimerText(seconds);
        while (seconds > 0 && !ransomwareEnded)
        {
            await DelayAttack(1000, GameDataManager.Screen.other);
            seconds--;
            timer.text = TimerText(seconds);
        }
        if (!ransomwareEnded)
        {
            isSuccess = false;
            ransomwareEnded = true;
        }
    }
    private string TimerText(int seconds)
    {
        return $"{seconds / 60:D2}:{seconds % 60:D2}";
    }
}