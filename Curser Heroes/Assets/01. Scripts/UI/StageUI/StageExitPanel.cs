using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageExitPanel : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI timeText;
    [SerializeField]private int countDownTime;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
        
    }

    private void OnEnable()
    {
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        int timer = countDownTime;
        while (timer > 0)
        {
            timeText.text = timer.ToString() + "초 후 전투 나가기";
            yield return new WaitForSeconds(1.0f);
            timer--;
        }
        gameObject.SetActive(false);
        SceneManager.LoadScene("98. CreatersScenes/JW_StageSelectUI");
        
    }
}
