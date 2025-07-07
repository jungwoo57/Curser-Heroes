using System;
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
        UpdateSkillScroll();
    }

    public void ClickTypeButton()
    {
        
    }

    public void ShowAttackSkill()
    {
        
    }

    public void UpdateSkillInfo()
    {
        if (!selectSkill) return;
        skillName.text = selectSkill.skillName;
        skillDescription.text = selectSkill.description;
        //skillEffect.text = selectSkill. 효과는 뭐라넣어야하지
    }

    public void UpdateSkillScroll()
    {
        for (int i = 0; i < GameManager.Instance.allSkills.Count; i++)
        {
            if (GameManager.Instance.allSkills[i])
            {
                skillButton[i].image.sprite = GameManager.Instance.allSkills[i].icon;
                if (GameManager.Instance.hasSkills.Find(n => n.skillName == GameManager.Instance.allSkills[i].name))
                {
                    skillButton[i].image.color = new Color(1f, 1f, 1f, 0.5f);
                }
                else
                {
                    skillButton[i].image.color = new Color(1f, 1f, 1f, 1.0f);
                }
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
            GameManager.Instance.hasSkills.Add(selectSkill);
            unlockButton.interactable = false;
        }
    }
}
