using System;
using System.Collections;
using System.Collections.Generic;
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

        attacks.Add(Phishing);
        // attacks.Add(Bot);
        // attacks.Add(Ransomware);
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
        nextAttackTime = UnityEngine.Random.Range(10,20);
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
            countdownCoroutine = StartCoroutine(ReturnBattle());
        }
        else
        {
            GameDataManager.instance.screen = lastScreen;
            SetNextAttackTime();
        }
    }
    private Coroutine countdownCoroutine;
    private IEnumerator ReturnBattle()
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


    //
    //これより下は擬似攻撃とそれに付随するメソッド
    //

    //フィッシング
    [Header("フィッシング")]
    [SerializeField] GameObject phishing;
    private void Phishing()
    {
        MailManager.instance.isPhishingMailAttacking = true;
    }
    public void PhishingEnd(bool isSuccess)
    {
        ShowResult(isSuccess);
    }


    //ボット
    [Header("ボット")]
    [SerializeField] GameObject bot;
    private Coroutine botEffectCoroutine;
    private void Bot()
    {
        botEffectCoroutine = StartCoroutine(BotEffect());
    }
    private IEnumerator BotEffect()
    {
        GameDataManager.instance.players[0].bulletInterval *= 2f;
        yield return WaitAttack(10);
        BotEnd(false);
    }
    private void BotEnd(bool isSuccess)
    {
        StopCoroutine(botEffectCoroutine);
        GameDataManager.instance.players[0].bulletInterval *= 1 / 2f;
        lastScreen = GameDataManager.instance.screen;
        ShowResult(isSuccess);
    }
    public void VirusBaster()
    {
        if (botEffectCoroutine != null)
        {
            BotEnd(true);
        }
    }

    //ランサムウェア
    [Header("ランサムウェア")]
    [SerializeField] GameObject ransomware;
    [SerializeField] TextMeshProUGUI timerText, money;
    private Coroutine timerCoroutine;
    private void Ransomware()
    {
        lastScreen = GameDataManager.instance.screen;
        GameDataManager.instance.screen = GameDataManager.Screen.other;
        ransomware.SetActive(true);
        timerCoroutine = StartCoroutine(TimerStart());
    }
    private void RansomwareEnd(bool isSuccess)
    {
        StopCoroutine(timerCoroutine);
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
    private IEnumerator TimerStart()
    {
        int seconds = 60;
        timerText.text = TimerText(seconds);
        while (seconds > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
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