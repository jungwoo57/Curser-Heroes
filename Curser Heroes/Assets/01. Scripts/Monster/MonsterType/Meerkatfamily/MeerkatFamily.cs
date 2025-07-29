using System.Collections;
using UnityEngine;

public class MeerkatFamily : BaseMonster
{
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private GameObject splitPrefab;
    [SerializeField] private int splitCount = 5;
    [SerializeField] private float delaytime = 0.7f;

    protected override void Attack()
    {
        StartCoroutine(AttackReadyTime());
    }
    
    [ContextMenu("분열패턴")]
    protected override void Die()
    {
        if (isDead) return;
        StartCoroutine(SpawnAfterDelay(delaytime));
        base.Die(); // 먼저 isDead 처리 (중복 방지)
        
    }
    
    private IEnumerator SpawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        float radius = 1.5f;
        int maxAttempts = 10;

        for (int i = 0; i < splitCount; i++)
        {
            Vector2 spawnPos = Vector2.zero;
            bool found = false;

            // 겹치지 않는 위치 찾기
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                Vector2 candidate = (Vector2)transform.position + Random.insideUnitCircle * radius;
                Collider2D overlap = Physics2D.OverlapCircle(candidate, 0.5f, LayerMask.GetMask("Monster"));
                if (overlap == null)
                {
                    spawnPos = candidate;
                    found = true;
                    break;
                }
            }

            if (!found)
                spawnPos = (Vector2)transform.position + Random.insideUnitCircle * radius;

            GameObject monster = Instantiate(splitPrefab, spawnPos, Quaternion.identity);

            if (monster.TryGetComponent<BaseMonster>(out var m))
            {
                m.Setup(new MonsterData
                {
                    maxHP = 35,
                    damage = 1,
                    attackCooldown = 2f,
                    valueCost = 1,
                    monsterPrefab = splitPrefab
                });

                WaveManager.Instance.RegisterMonster(monster);
                m.onDeath += WaveManager.Instance.OnMonsterKilled;
            }
        }
    }

    IEnumerator AttackReadyTime() // 1.0초후에 공격
    {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(AttackReadyTime());
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider != null)
        {
            if (WeaponManager.Instance != null)
            {
                WeaponManager.Instance.TakeWeaponLifeDamage();
                Debug.Log("근접 공격");
            }
            else
            {
                Debug.LogWarning("WeaponManager 인스턴스를 찾을 수 없습니다!");
            }
        }
    }
}
