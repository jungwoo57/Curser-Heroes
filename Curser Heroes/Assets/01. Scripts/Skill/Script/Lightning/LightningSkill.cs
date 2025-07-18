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

    public void TryTriggerLightning(Monster hitMonster)
    {
        if (skillInstance == null) return;

        float procChance = 0.1f; // 10%

        if (Random.value > procChance)
            return;

        SkillLevelData levelData = skillInstance.skill.levelDataList[skillInstance.level - 1];
        int damage = levelData.damage;
        float skillRange = levelData.sizeMultiplier; // 범위 용도로 사용

        List<Monster> targets = GetMonstersInRange(hitMonster.transform.position, skillRange);

        hitMonster.TakeDamage(damage);

        foreach (var target in targets)
        {
            if (target == hitMonster) continue;

            target.TakeDamage(damage);

            SpawnLightningEffect(hitMonster.transform.position, target.transform.position);
        }
    }

    List<Monster> GetMonstersInRange(Vector3 center, float range)
    {
        // 2D 물리용 OverlapCircleAll 사용
        Collider2D[] colliders = Physics2D.OverlapCircleAll(center, range, monsterLayerMask);
        List<Monster> monsters = new List<Monster>();
        //foreach (var col in colliders)
        //{
        //    Monster m = col.GetComponent<Monster>();
        //    if (m != null && m.IsAlive())
        //        monsters.Add(m);
        //}
        return monsters;
    }

    void SpawnLightningEffect(Vector3 from, Vector3 to)
    {
        if (lightningEffectPrefab == null) return;

        Vector3 dir = to - from;
        float distance = dir.magnitude;

        GameObject lightning = Instantiate(lightningEffectPrefab, from, Quaternion.identity);

        // 2D에서는 Z축 회전으로 방향 맞춤
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        lightning.transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // -90도 보정 필요할 수 있음

        Vector3 scale = lightning.transform.localScale;
        scale.y = distance;
        lightning.transform.localScale = scale;
    }
}