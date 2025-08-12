
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SkillInfoPanelUI : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI skillMaxLevel;
    public TextMeshProUGUI skillTypeText;
    public Image skillIcon;
    
    
    public void UpdateUI(SkillData data)
    {
        skillIcon.sprite = data.icon;
        skillName.text = data.skillName;
        skillDescription.text= data.description;
        switch (data.type)
        {
            case SkillType.Attack:
                skillTypeText.text = "공격형";
                break;
            case SkillType.Defense:
                skillTypeText.text = "수비형";
                break;
            case SkillType.Buff:
                skillTypeText.text = "버프형";
                break;
        }
        
        skillMaxLevel.text = "최대 레벨 : " + data.levelDataList.Count;
       
    }
}
