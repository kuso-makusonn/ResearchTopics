using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mail", menuName = "Create New Mail")]
public class MailEntity : ScriptableObject
{
    public string title, sender, main, link;
    public List<MailModel.DiscountItemTemplate> discountItems;
    //脱獄なんて積分使えばすぐ！\nでも君たちは積分を習ってないからできないんだ。\nそんな君たちに積分を完璧にマスターできる講座を受けられるチャンスを用意しました！\n1回たった100,000円！\nさらにあなたを私たちのメンバーに招待します！\n入会料は50,000円！\n高いと思いましたか？心配ありません！なんと、この講座を人に紹介するごとに10.000円をゲットできます！詳しくは下のリンクから！
}
