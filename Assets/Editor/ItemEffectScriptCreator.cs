using UnityEditor;

public static class ItemEffectScriptCreator
{
    [MenuItem("Assets/Create/Item-Effect Script", false, 80)]
    public static void CreateItemEffectScript()
    {
        string templatePath = "Assets/Editor/ScriptTemplates/ItemEffectTemplate.txt";
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewItemEffect.cs");
    }
}