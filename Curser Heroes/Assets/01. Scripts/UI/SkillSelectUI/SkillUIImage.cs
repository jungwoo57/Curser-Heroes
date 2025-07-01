using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIImage : MonoBehaviour
{
    public Image skillImage;
    public SkillData data;
    private StageSkillSelectUI stageSkillSelectUI;

    private void Awake()
    {
        stageSkillSelectUI = GetComponentInParent<StageSkillSelectUI>();
    }

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
        if (stageSkillSelectUI != null)
        {
            stageSkillSelectUI.stageSelectedSkillUI.SelectSkill(data);
        }
        else
        {
            Debug.Log("부모 오브젝트 못찾음");
        }
        //데이터 넘겨주기
    }
}
