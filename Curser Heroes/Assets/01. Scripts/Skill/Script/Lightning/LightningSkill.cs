using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;
    public GameObject lightningEffectPrefab;
    public GameObject lightningStrikePrefab;
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

            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && skillInstance.skill.audioClip != null)
            {
                audioSource.PlayOneShot(skillInstance.skill.audioClip);
            }

            // 1. 피격된 몬스터에게 떨어지는 번개 생성 (회전 없음)
            // 이펙트 시작 위치는 몬스터의 머리 위로 조정할 수 있습니다.
            Vector3 strikePosition = hitMonster.transform.position + Vector3.up * 0.5f; // 예시
            GameObject strikeEffect = Instantiate(lightningStrikePrefab, strikePosition, Quaternion.identity);
            Destroy(strikeEffect, 1f); // 1초 후 파괴

            Collider2D[] hits = Physics2D.OverlapCircleAll(hitMonster.transform.position, 1f, monsterLayerMask);
            Debug.Log($"근처 몬스터 탐지 수: {hits.Length}");

            int damage = skillInstance.skill.levelDataList[skillInstance.level - 1].damage;

            // 2. 다른 몬스터에게 전이되는 번개 생성 (회전 및 스케일 적용)
            foreach (var col in hits)
            {
                // 보스 및 일반 몬스터 중 hitMonster 제외하고 처리
                if (col.gameObject != hitMonster.gameObject)
                {
                    Vector3 start = hitMonster.transform.position;
                    Vector3 end = col.transform.position;
                    Vector3 direction = end - start;
                    float distance = direction.magnitude;

                    GameObject effect = Instantiate(lightningEffectPrefab, start, Quaternion.identity);

                    Transform visual = effect.transform.Find("Visual");
                    if (visual != null)
                    {
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        visual.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

                        Vector3 scale = visual.localScale;
                        scale.y = distance;
                        visual.localScale = scale;
                    }

                    // 데미지 적용
                    if (col.TryGetComponent<BaseMonster>(out var m))
                    {
                        m.TakeDamage(damage);
                    }
                    else if (col.TryGetComponent<BossStats>(out var boss))
                    {
                        boss.TakeDamage(damage);
                    }

                    Destroy(effect, 1f);
                    Debug.Log($"라이트닝 데미지 {damage} 입힘 to {col.gameObject.name}");
                }
            }
        }
    }
}