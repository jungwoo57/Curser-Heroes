using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LabPanel : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI skillEffect;
    public SkillData selectSkill;

    public void ClickTypeButton()
    {
        
    }

    public void ShowAttackSkill()
    {
        
    }

    public void UpdateSkillInfo()
    {
        skillName.text = selectSkill.skillName;
        skillDescription.text = selectSkill.description;
        //skillEffect.text = selectSkill. 효과는 뭐라넣어야하지
    }
}
