using UnityEngine;

[CreateAssetMenu(fileName = "Mail", menuName = "Create New Mail")]
public class MailEntity : ScriptableObject
{
    public string title, sender, main;
}
