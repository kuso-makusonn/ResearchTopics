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
    };



    public static bool CanPurchase(int price)
    {
        return GameDataManager.instance.money >= price;
    }

    public static void CancelAllEffect()
    {
        foreach (List<CancellationTokenSource> ctsList in ctsLists)
        {
            foreach (CancellationTokenSource cts in ctsList)
            {
                cts.Cancel();
            }
        }
    }
    public static void CancelEffect(List<CancellationTokenSource> ctsList)
    {
        foreach (CancellationTokenSource cts in ctsList)
        {
            cts.Cancel();
        }
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
            Debug.Log("おぅ、飲み行こ飲み行こ");
            await Task.Delay((int)(duration * 1000), cts.Token);
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
}
