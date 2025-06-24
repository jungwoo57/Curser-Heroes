using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SkillUI : MonoBehaviour
{
    [SerializeField] private Image skillImage;
    [SerializeField] private TextMeshProUGUI skillName;

    public void UpdateSkillUI()
    {
        //skillImage.sprite = skillImage.sprite;   Skilldata에서 이미지 가져와서 적용
        //skillName.text = skill.name + " : " + skill.lv;                Skilldata에서 이름 + 레벨 가져와서 적용
    }
}
