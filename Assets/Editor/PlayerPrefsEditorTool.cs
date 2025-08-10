#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// PlayerPrefsをインスペクタ上で操作できるエディタウィンドウ
/// </summary>
public class PlayerPrefsEditorTool : EditorWindow
{
    private Vector2 scrollPos;

    [MenuItem("Tools/PlayerPrefs Manager")]
    public static void ShowWindow()
    {
        GetWindow<PlayerPrefsEditorTool>("PlayerPrefs Manager");
    }

    void OnGUI()
    {
        GUILayout.Label("PlayerPrefs 管理ツール", EditorStyles.boldLabel);

        if (GUILayout.Button("すべて削除"))
        {
            if (EditorUtility.DisplayDialog("確認", "本当にPlayerPrefsをすべて削除しますか？", "はい", "いいえ"))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                Debug.Log("PlayerPrefsをすべて削除しました。");
            }
        }

        GUILayout.Space(10);
        GUILayout.Label("保存されているデータ（未実装）：", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        //出来たらリスト表示

        EditorGUILayout.EndScrollView();
    }
}
#endif