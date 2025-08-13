using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText, moneyText;
    public List<PlayerData> players { get; private set; } = new();
    public int score { get; private set; }
    public int money { get; private set; }

    public int CreateNewPlayerData()
    {
        PlayerData player = new();
        players.Add(player);
        return players.Count - 1;
    }
    public void ResetScore()
    {
        score = 0;
        ShowScore();
    }

    public void ScoreUp(int amount)
    {
        score += amount;
    }

    private void ShowScore()
    {
        scoreText.text = "Score:" + score.ToString();
    }
    public void MoneyUp(int amount)
    {
        money += amount;
        ShowMoney();
    }
    private void ShowMoney()
    {
        moneyText.text = "所持金:" + money.ToString() + "円";
    }
    public void Heal(int amount)
    {
    }
}