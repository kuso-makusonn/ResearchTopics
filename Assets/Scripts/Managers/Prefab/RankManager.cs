using TMPro;
using UnityEngine;

public class RankManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rankText, nameText, scoreText;
    [SerializeField] GameObject highlight;
    public void Init(int rank, string name, int score)
    {
        rankText.text = rank.ToString();
        nameText.text = name;
        scoreText.text = score.ToString();
    }
    public void Null(int rank)
    {
        rankText.text = rank.ToString();
        nameText.text = "-";
        scoreText.text = "-";
    }
    public void Highlight(bool isHighlight)
    {
        highlight.SetActive(isHighlight);
    }
}
