using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class WARIAINISITAIYONE : MonoBehaviour
{
    [ContextMenu("WARIAINIHENKOU")]
    void WARIAINIHENKOU()
    {
        Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
        int count = 0;

        foreach (Canvas canvas in canvases)
        {
            RectTransform rootRT = canvas.GetComponent<RectTransform>();
            if (rootRT != null)
            {
                count += ConvertRecursively(rootRT);
            }
        }

        Debug.Log($"{count} 個の RectTransform をストレッチ（親からの割合）に変換しました。");
    }

    // 子 → 親の順に再帰的に変換処理
    int ConvertRecursively(RectTransform rt)
    {
        int localCount = 0;

        foreach (Transform child in rt)
        {
            RectTransform childRT = child as RectTransform;
            if (childRT != null)
            {
                localCount += ConvertRecursively(childRT);
            }
        }

        // Canvas 自身は除外
        if (rt.GetComponent<Canvas>() != null)
            return localCount;

        if (rt.offsetMax == Vector2.zero && rt.offsetMin == Vector2.zero)
            return localCount;

        RectTransform parentRT = rt.parent as RectTransform;
        if (parentRT == null)
            return localCount;

        Vector2 parentSize = parentRT.rect.size;
        if (parentSize.x == 0 || parentSize.y == 0)
            return localCount;

        SetAnchorPresetsToStretchKeepVisual(rt);

        TextMeshProUGUI text = rt.GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            float w = text.fontSize / rt.rect.size.y;
        }

        // offset から余白取得
        float left = rt.offsetMin.x;
        float bottom = rt.offsetMin.y;
        float right = -rt.offsetMax.x;
        float top = -rt.offsetMax.y;

        // 親サイズに対する割合に変換
        rt.anchorMin = new Vector2(left / parentSize.x, bottom / parentSize.y);
        rt.anchorMax = new Vector2(1 - right / parentSize.x, 1 - top / parentSize.y);

        // オフセットをリセットしてストレッチに
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.sizeDelta = Vector2.zero;

        localCount++;
        return localCount;
    }
    void SetAnchorPresetsToStretchKeepVisual(RectTransform rectTransform)
    {
        if (rectTransform == null || rectTransform.parent == null)
        {
            Debug.LogWarning("RectTransform またはその親が null です。");
            return;
        }

        RectTransform parentRect = rectTransform.parent as RectTransform;
        Vector2 parentSize = parentRect.rect.size;

        // ワールド空間での 4 つのコーナーを取得
        Vector3[] worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);
        Vector3 worldBL = worldCorners[0]; // Bottom-Left
        Vector3 worldTR = worldCorners[2]; // Top-Right

        // 親のローカル空間に変換
        Vector3 localBL = parentRect.InverseTransformPoint(worldBL);
        Vector3 localTR = parentRect.InverseTransformPoint(worldTR);

        // 元のピボットを取得
        Vector2 pivot = rectTransform.pivot;

        // Stretch アンカーに変更
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);

        // 新しいサイズと位置を計算
        float newWidth = localTR.x - localBL.x;
        float newHeight = localTR.y - localBL.y;
        Vector2 newSizeDelta = new Vector2(newWidth - parentSize.x, newHeight - parentSize.y);

        float newPosX = (localBL.x + localTR.x) / 2 - parentSize.x * (0.5f - pivot.x);
        float newPosY = (localBL.y + localTR.y) / 2 - parentSize.y * (0.5f - pivot.y);

        rectTransform.sizeDelta = newSizeDelta;
        rectTransform.anchoredPosition = new Vector2(newPosX, newPosY);
    }
}