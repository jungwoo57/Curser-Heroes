using UnityEngine;

public class StrengthTrainingSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;

    public void Init(SkillManager.SkillInstance instance)
    {
        skillInstance = instance;
    }

    // 현재 레벨에 따른 데미지 증가량 반환
    public int GetDamageIncrease()
    {
        if (skillInstance == null) return 0;

        int levelIndex = Mathf.Clamp(skillInstance.level - 1, 0, skillInstance.skill.levelDataList.Count - 1);
        return skillInstance.skill.levelDataList[levelIndex].damage;
    }
}
