using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SkillUI : MonoBehaviour
{
    [SerializeField] private Image skillImage;
    [SerializeField] private TextMeshProUGUI skillName;

    public void SetSkill(SkillData.SkillInstance instance)
    {
        skillImage.sprite = instance.skill.icon;
        skillName.text = $"{instance.skill.skillName} Lv.{instance.level}";
    }
}
