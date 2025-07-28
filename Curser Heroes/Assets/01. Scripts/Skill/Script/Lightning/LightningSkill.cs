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

        float procChance = 1f;
        if (UnityEngine.Random.value < procChance)
        {
            Debug.Log("라이트닝 스킬 발동!");

            Collider2D[] hits = Physics2D.OverlapCircleAll(hitMonster.transform.position, 1f, monsterLayerMask);
            Debug.Log($"근처 몬스터 탐지 수: {hits.Length}");

            int damage = skillInstance.skill.levelDataList[skillInstance.level - 1].damage;

            foreach (var col in hits)
            {
                // 보스 및 일반 몬스터 중 hitMonster 제외하고 처리
                if (col.TryGetComponent<BaseMonster>(out var m) && m != hitMonster)
                {
                    Debug.Log($"라이트닝 대상: {m.gameObject.name} 위치: {m.transform.position}");

                    Vector3 start = hitMonster.transform.position;
                    Vector3 end = m.transform.position;
                    Vector3 direction = end - start;
                    float distance = direction.magnitude;

                    GameObject effect = Instantiate(lightningEffectPrefab, start, Quaternion.identity);

                    Transform visual = effect.transform.Find("Visual");
                    if (visual != null)
                    {
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        visual.rotation = Quaternion.Euler(0f, 0f, angle);

                        Vector3 scale = visual.localScale;
                        scale.y = distance;
                        visual.localScale = scale;
                    }

                    m.TakeDamage(damage);
                    Destroy(effect, 1f);

                    Debug.Log($"라이트닝 데미지 {damage} 입힘 to {m.gameObject.name}");
                    continue;
                }

                if (col.TryGetComponent<BossStats>(out var boss) && col.gameObject != hitMonster.gameObject)
                {
                    Debug.Log($"라이트닝 대상(보스): {boss.gameObject.name} 위치: {boss.transform.position}");

                    Vector3 start = hitMonster.transform.position;
                    Vector3 end = boss.transform.position;
                    Vector3 direction = end - start;
                    float distance = direction.magnitude;

                    GameObject effect = Instantiate(lightningEffectPrefab, start, Quaternion.identity);

                    Transform visual = effect.transform.Find("Visual");
                    if (visual != null)
                    {
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        visual.rotation = Quaternion.Euler(0f, 0f, angle);

                        Vector3 scale = visual.localScale;
                        scale.y = distance;
                        visual.localScale = scale;
                    }

                    boss.TakeDamage(damage);
                    Destroy(effect, 1f);

                    Debug.Log($"라이트닝 데미지 {damage} 입힘 to 보스 {boss.gameObject.name}");
                }
            }
        }
    }
}