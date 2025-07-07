using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LabPanel : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI skillEffect;
    public SkillData selectSkill;
    public Button[] skillButton;
    public Button unlockButton;


    private void OnEnable()
    {
        UpdateSkillScroll(SkillType.Attack);
    }

    public void ClickTypeButton(int num)
    {
        SkillType type = (SkillType)num;
        UpdateSkillScroll(type);
    }
    
    public void UpdateSkillInfo()
    {
        if (!selectSkill) return;
        skillName.text = selectSkill.skillName;
        skillDescription.text = selectSkill.description;
        //skillEffect.text = selectSkill. 효과는 뭐라넣어야하지
    }

    public void UpdateSkillScroll(SkillType type)
    {
        int skillindex = 0;
        for (int i = 0; i < skillButton.Length; i++)  //이미지 초기화
        {
            skillButton[i].image.sprite = null;
        }
        
        for (int i = 0; i < GameManager.Instance.allSkills.Count; i++)  // 타입별로 정렬
        {
            if (GameManager.Instance.allSkills[i]&&GameManager.Instance.allSkills[i].type==type)
            {
                skillButton[skillindex].image.sprite = GameManager.Instance.allSkills[i].icon;
                if (GameManager.Instance.hasSkills.Find(n => n.skillName == GameManager.Instance.allSkills[i].name))
                {
                    skillButton[i].image.color = new Color(1f, 1f, 1f, 0.5f);
                }
                else
                {
                    skillButton[i].image.color = new Color(1f, 1f, 1f, 1.0f);
                }
                skillindex++;
            }
        }
    }

    public void ClickSkillButton(int skillIndex)
    {
        selectSkill = GameManager.Instance.allSkills[skillIndex];
        UpdateSkillInfo();
        if(GameManager.Instance.hasSkills.Find(n => n.skillName == selectSkill.skillName))
        {
            unlockButton.interactable = false;
        }
        else
        {
            unlockButton.interactable = true;
        }
    }

    public void UnlockSkill()
    {
        if (GameManager.Instance.hasSkills.Find(n => n.skillName == selectSkill.skillName))
        {
            Debug.Log("해당스킬이 있습니다");
        }
        else
        {
            GameManager.Instance.UnlockSkill(selectSkill);
            unlockButton.interactable = false;
        }
    }
}
