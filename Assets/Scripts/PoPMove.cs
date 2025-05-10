using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PoPMove : MonoBehaviour
{
    public Vector3 targetPosition; // 目標位置
    public Vector3 startPosition; // 初めの位置
    public float speed = 200f;     // 移動速度（UIはピクセルベース）

    private RectTransform rectTransform;
    public static bool Reseter=true;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // 現在位置
        Vector3 currentPosition = rectTransform.anchoredPosition;

        // targetPosition まで徐々に移動
        Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        rectTransform.anchoredPosition = newPosition;

        // 目的地に到達したら移動停止
        if (Vector3.Distance(newPosition, targetPosition) < 0.1f)
        {
            // 必要があればフラグなどで停止処理
            //Debug.Log("目的地に到達しました！");
        }
        if(Reseter==false){
            ResetPop();
        }
    }
    public void ResetPop(){
            rectTransform.position=startPosition;
            Reseter = !Reseter;
    }
}
