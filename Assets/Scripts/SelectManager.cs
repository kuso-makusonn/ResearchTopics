using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            TitleManager.SceneChanger = true;
            SceneManager.LoadScene("TitleScene");
        }
    }
    public void Mail()
    {
        SceneManager.LoadScene("MailScene");
    }
    public void Web()
    {
        SceneManager.LoadScene("WebSimScene");
    }
    public void SNS(){
        SceneManager.LoadScene("SNSSimScene");
    }
}
