using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIImage : MonoBehaviour
{
    public Image skillImage;
    public SkillData data;

    public void UpdateUI(SkillData skillData)
    {
            data = skillData;
            if (data.icon == null)
            {
                Debug.Log("데이터 없음");
                return;
            }
            skillImage.sprite = data.icon;
        
    }

    public void OnClickSkillButton()
    {
        //데이터 넘겨주기
    }
}
