using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSkill : MonoBehaviour
{
    public void Init(SkillManager.SkillInstance skillInstance)
    {
        var levelData = skillInstance.skill.levelDataList[skillInstance.level - 1];
        int count = levelData.count;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject obj = Instantiate(skillInstance.skill.skillPrefab, transform.position, rotation, transform);

            obj.transform.localScale *= levelData.sizeMultiplier;

            if (obj.TryGetComponent(out SkillProjectile proj))
                proj.Init(levelData.damage);
        }
    }
}
