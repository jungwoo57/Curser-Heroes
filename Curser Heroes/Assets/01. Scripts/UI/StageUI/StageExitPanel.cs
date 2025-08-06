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

        CloseAllUIWindows();

        gameObject.SetActive(false);
        SceneManager.LoadScene("98. CreatersScenes/JW_StageSelectUI");
        
    }
    private void CloseAllUIWindows()
    {
        // 스킬 선택 UI를 찾아서 닫습니다.
        var skillSelectUI = GameObject.FindObjectOfType<SkillSelectUI>();
        if (skillSelectUI != null)
        {
            Destroy(skillSelectUI.gameObject);
        }

        // 보상 선택 UI를 찾아서 닫습니다.
        var rewardSelectUI = GameObject.FindObjectOfType<RewardSelectUI>();
        if (rewardSelectUI != null)
        {
            Destroy(rewardSelectUI.gameObject);
        }
    }
}
