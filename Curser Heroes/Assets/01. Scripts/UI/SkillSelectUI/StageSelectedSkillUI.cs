using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageSelectedSkillUI : MonoBehaviour
{
   public SkillData[] skills;
   public SkillUIImage[] skillImages;
   public Button applyButton;
   
   public void SelectSkill(SkillData skill)       //스킬창에 고른 스킬 보여주기
   {
      Debug.Log("ㅎㅇㅎㅇ");
      for (int i = 0; i < skillImages.Length; i++)
      {
         if (skills[i] == skill)
         {
            skillImages[i].CancleSelect();
            Debug.Log("동일한 스킬이 장착되어있습니다" + i);
            return;
         }
         if (!skillImages[i].data)
         {
            skillImages[i].UpdateUI(skill);
            skills[i] = skillImages[i].data;
            Debug.Log(i);
            return;
         }
      }
   }

   public void OnClickApplyButton()        //위치 변경 예정
   {
      for (int i = 0; i < skills.Length; i++)   //스킬 12개 이하면 리턴
      {
         if (!skills[i])
         {
            Debug.Log("12개의 스킬이 선택되지 않았습니다");
            return;
         }
      }
      GameManager.Instance.EquipSkill(skills);
   }
}
