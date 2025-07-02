using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SkillInfoPanelUI : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI skillBaseDamage;
    public TextMeshProUGUI skillLevelPerDamage;
    public TextMeshProUGUI skillMaxLevel;
    public TextMeshProUGUI skillAddEffect;
    public Image skillIcon;
    
    
    public void UpdateUI(SkillData data)
    {
        skillIcon.sprite = data.icon;
        skillName.text = data.skillName;
        skillDescription.text= data.description;
        skillBaseDamage.text = "Lv.1 피해랑: " + data.levelDataList.ToString();
        //skillLevelPerDamage.text = data.   레벨당 데미지 추가
        skillMaxLevel.text = "최대 레벨 : " + data.maxLevel.ToString();
        //skillAddEffect.text = data.        데이터 추가 효과 추가
    }
}
