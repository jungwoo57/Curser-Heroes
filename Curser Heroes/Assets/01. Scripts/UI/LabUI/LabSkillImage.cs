using UnityEngine;
using UnityEngine.UI;
public class LabSkillImage : MonoBehaviour
{
    public Image skillImage;
    public Image backGroundImage;
    public void Init(SkillData skillData)
    {
        skillImage.sprite = skillData.icon;
    }
}
