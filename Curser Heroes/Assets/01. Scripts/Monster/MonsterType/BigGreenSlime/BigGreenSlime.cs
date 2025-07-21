using System.Collections;
using UnityEngine;

public class BigGreenSlime : BaseMonster
{
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private GameObject splitPrefab;
    [SerializeField] private int splitCount = 5;

    protected override void Attack()
    {
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider != null)
        {
            WeaponManager.Instance?.TakeWeaponLifeDamage();
            Debug.Log("BigGreenSlime 자폭 공격! 무기 내구도 감소");
        }

        currentHP = 0;  // 자폭 처리를 위해 강제 체력 0
        Die();
    }

    protected override void Die()
    {
        if (isDead) return;
        StartCoroutine(SpawnAfterDelay(0.2f));
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

            GameObject slime = Instantiate(splitPrefab, spawnPos, Quaternion.identity);

            if (slime.TryGetComponent<BaseMonster>(out var m))
            {
                m.Setup(new MonsterData
                {
                    maxHP = 10,
                    damage = 1,
                    attackCooldown = 2f,
                    valueCost = 1,
                    monsterPrefab = splitPrefab
                });

                WaveManager.Instance.RegisterMonster(slime);
                m.onDeath += WaveManager.Instance.OnMonsterKilled;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
