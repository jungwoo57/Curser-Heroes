using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;
    public GameObject lightningEffectPrefab;
    public LayerMask monsterLayerMask;

    public void Init(SkillManager.SkillInstance instance)
    {
        skillInstance = instance;
    }
    public void TryTriggerLightning(BaseMonster hitMonster)
    {
        if (skillInstance == null) return;

        float procChance = 0.1f;
        if (UnityEngine.Random.value < procChance)
        {
            Debug.Log("라이트닝 스킬 발동!");

            Collider2D[] hits = Physics2D.OverlapCircleAll(hitMonster.transform.position, 1f, monsterLayerMask);
            Debug.Log($"근처 몬스터 탐지 수: {hits.Length}");

            foreach (var col in hits)
            {
                BaseMonster m = col.GetComponent<BaseMonster>();
                if (m != null && m != hitMonster)
                {
                    Debug.Log($"라이트닝 대상: {m.gameObject.name} 위치: {m.transform.position}");

                    GameObject effect = Instantiate(lightningEffectPrefab, m.transform.position, Quaternion.identity);
                    Destroy(effect, 1f);

                    int damage = skillInstance.skill.levelDataList[skillInstance.level - 1].damage;
                    m.TakeDamage(damage);

                    Debug.Log($"라이트닝 데미지 {damage} 입힘 to {m.gameObject.name}");
                }
            }
        }
    }
}