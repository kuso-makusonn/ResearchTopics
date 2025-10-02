using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class ItemEffectManager
{
    private static List<List<CancellationTokenSource>> ctsLists = new()
    {
        AcceptDrinkingInvitationCTS,
        temporaryBulletPowerUpCTS,
        temporaryBulletIntervalCTS,
        temporaryPlayerMoveSpeedUpCTS
    };



    public static bool CanPurchase(int price)
    {
        return GameDataManager.instance.money >= price;
    }

    public static void CancelAllEffect()
    {
        if (ctsLists == null) return;
        foreach (List<CancellationTokenSource> ctsList in ctsLists)
        {
            CancelEffect(ctsList);
        }
    }
    public static void CancelEffect(List<CancellationTokenSource> ctsList)
    {
        if (ctsList == null) return;
        foreach (CancellationTokenSource cts in ctsList)
        {
            cts.Cancel();
            cts.Dispose();
            ctsList.Remove(cts);
        }
    }
    public static async Task ItemUsingDelay(float duration, CancellationTokenSource cts)
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
                timer += (float)interval / 1000;
            }
        }
    }
    public static bool CheckPlayerIndex(int playerIndex)
    {
        return playerIndex >= 0
        || playerIndex <= GameDataManager.instance.players.Count - 1;
    }

    //
    //これより下はアイテム効果のメソッド
    //ItemEffectを継承したアイテム効果クラスのApplyEffect()で使用する
    //効果時間があるものは複雑
    //ないものは普通に実装してどうぞ
    //


    //効果時間があるもののテンプレート
    public static List<CancellationTokenSource> AcceptDrinkingInvitationCTS = new();
    public static async void AcceptDrinkingInvitation(float duration)
    {
        CancellationTokenSource cts = new();
        AcceptDrinkingInvitationCTS.Add(cts);
        try
        {
            Debug.Log("おぅ、飲み行こ飲み行こ！");
            await ItemUsingDelay(duration, cts);
            Debug.Log("楽しかったなぁ！");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("AcceptDrinkingInvitationはキャンセルされました");
            Debug.Log("もう帰るのかよ。ノリ悪いなぁ");
        }
        finally
        {
            AcceptDrinkingInvitationCTS.Remove(cts);
            cts.Dispose();
        }
    }


    //弾の威力アップ（永久的）
    public static void BulletPowerUp(int playerIndex, int amount)
    {
        if (!CheckPlayerIndex(playerIndex)) return;
        GameDataManager.instance.players[playerIndex].bulletPower += amount;
    }

    //弾の威力アップ（一時的）
    public static List<CancellationTokenSource> temporaryBulletPowerUpCTS = new();
    public static async void TemporaryBulletPowerUp(int playerIndex, int amount, float duration)
    {
        CancellationTokenSource cts = new();
        temporaryBulletPowerUpCTS.Add(cts);
        try
        {
            if (!CheckPlayerIndex(playerIndex)) return;
            BulletPowerUp(playerIndex, amount);
            await ItemUsingDelay(duration, cts);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("temporaryBulletPowerUpCTSはキャンセルされました");
        }
        finally
        {
            BulletPowerUp(playerIndex, -amount);
            temporaryBulletPowerUpCTS.Remove(cts);
            cts.Dispose();
        }
    }


    //弾の間隔短縮（永久的）
    public static void BulletInterval(int playerIndex, float fact)
    {
        if (!CheckPlayerIndex(playerIndex)) return;
        GameDataManager.instance.players[playerIndex].bulletInterval *= fact;
    }

    //弾の間隔短縮（一時的）
    public static List<CancellationTokenSource> temporaryBulletIntervalCTS = new();
    public static async void TemporaryBulletInterval(int playerIndex, float fact, float duration)
    {
        CancellationTokenSource cts = new();
        temporaryBulletIntervalCTS.Add(cts);
        try
        {
            if (!CheckPlayerIndex(playerIndex)) return;
            BulletInterval(playerIndex, fact);
            await ItemUsingDelay(duration, cts);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("temporaryBulletIntervalCTSはキャンセルされました");
        }
        finally
        {
            BulletInterval(playerIndex, 1 / fact);
            temporaryBulletIntervalCTS.Remove(cts);
            cts.Dispose();
        }
    }


    //移動速度アップ（永久的）
    public static void PlayerMoveSpeedUp(int playerIndex, float fact)
    {
        if (!CheckPlayerIndex(playerIndex)) return;
        GameDataManager.instance.players[playerIndex].moveSpeed *= fact;
    }

    //移動速度アップ（一時的）
    public static List<CancellationTokenSource> temporaryPlayerMoveSpeedUpCTS = new();
    public static async void TemporaryPlayerMoveSpeedUp(int playerIndex, float fact, float duration)
    {
        CancellationTokenSource cts = new();
        temporaryPlayerMoveSpeedUpCTS.Add(cts);
        try
        {
            if (!CheckPlayerIndex(playerIndex)) return;
            PlayerMoveSpeedUp(playerIndex, fact);
            await ItemUsingDelay(duration, cts);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("temporaryPlayerMoveSpeedUpCTSはキャンセルされました");
        }
        finally
        {
            PlayerMoveSpeedUp(playerIndex, 1 / fact);
            temporaryPlayerMoveSpeedUpCTS.Remove(cts);
            cts.Dispose();
        }
    }


    //ウイルスバスター残り回数増加
    public static void VirusBusterCountUp(int amount)
    {
        GameDataManager.instance.canVirusBusterCount += amount;
    }
}
