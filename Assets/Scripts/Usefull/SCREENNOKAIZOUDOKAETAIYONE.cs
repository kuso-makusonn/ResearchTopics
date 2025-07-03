using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCREENNOKAIZOUDOKAETAIYONE : MonoBehaviour
{
    [SerializeField] RectTransform canvas;
    [SerializeField] float x, y;
    [ContextMenu("SCREENNOKAIZOUDOHENKOU")]
    IEnumerator SCREENNOKAIZOUDOHENKOU()
    {
        CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();

        float motoX = canvasScaler.referenceResolution.x;
        float motoY = canvasScaler.referenceResolution.y;
        float wX = x / motoX;
        float wY = y / motoY;
        float w = (wX + wY) / 2;

        canvasScaler.referenceResolution = new Vector2(x, y);

        yield return null; // 1フレーム待ってレイアウト更新を待つ

        TextMeshProUGUI[] texts = canvas.GetComponentsInChildren<TextMeshProUGUI>(true);
        Debug.Log($"Text数: {texts.Length}");

        foreach (TextMeshProUGUI text in texts)
        {
            text.ForceMeshUpdate(); // 明示的にレイアウト更新

            if (text.textInfo.characterCount > 0)
            {
                float actualSize = text.textInfo.characterInfo[0].pointSize;

                // オートサイズ解除
                text.enableAutoSizing = false;

                // スケーリング
                text.fontSize = actualSize * w;
            }
        }
    }
}
