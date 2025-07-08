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
    public TextMeshProUGUI hasJewelText;
    public TextMeshProUGUI useJewelText;
    private void OnEnable()
    {
        UpdateSkillScroll(SkillType.Attack);
        unlockButton.interactable = false;
        hasJewelText.text = GameManager.Instance.GetJewel().ToString();
        useJewelText.text = "";
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
        hasJewelText.text = GameManager.Instance.GetJewel().ToString();
        useJewelText.text = selectSkill.unlockCost.ToString();
        
    }

    public void UpdateSkillScroll(SkillType type)
    {
       // int skillindex = 0;
        for (int i = 0; i < skillButton.Length; i++)  //이미지 초기화
        {
            skillButton[i].image.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < GameManager.Instance.allSkills.Count; i++)  // 타입별로 정렬
        {
            if (GameManager.Instance.allSkills[i]&&GameManager.Instance.allSkills[i].type==type)
            {
                skillButton[i].image.gameObject.SetActive(true);
                skillButton[i].image.sprite = GameManager.Instance.allSkills[i].icon;
                if (GameManager.Instance.hasSkills.Find(n => n.skillName == GameManager.Instance.allSkills[i].skillName))
                {
                    skillButton[i].image.color = new Color(1f, 1f, 1f, 1.0f);
                }
                else
                {
                    skillButton[i].image.color = new Color(1f, 1f, 1f, 0.2f);
                }
                //skillindex++;
            }
        }
    }

    public void ClickSkillButton(int skillIndex)
    {
        selectSkill = GameManager.Instance.allSkills[skillIndex];
        UpdateSkillInfo();
        if(GameManager.Instance.hasSkills.Find(n => n.skillName == selectSkill.skillName)
           && GameManager.Instance.GetJewel() >= selectSkill.unlockCost)
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

    public void ClickExitButton()
    {
        gameObject.SetActive(false);
    }
}
