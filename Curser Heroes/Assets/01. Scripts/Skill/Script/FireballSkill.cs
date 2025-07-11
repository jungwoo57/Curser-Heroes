using System.Collections.Generic;
using UnityEngine;

public class FireballSkill : MonoBehaviour
{
    private int damage;
    private float speed = 10f;
    private Vector3 direction;
    private HashSet<Component> damagedTargets = new HashSet<Component>();

    // 화염구 초기화: 데미지와 목표 위치를 받아서 방향 계산
    private HashSet<BaseMonster> damagedMonsters = new HashSet<BaseMonster>();

    public void Init(int damage, Vector3 dir)
    {
        this.damage = damage;
        this.direction = dir.normalized;
        Destroy(gameObject, 5f); // 수명 제한
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out BaseMonster monster) && !damagedTargets.Contains(monster))
        {
            monster.TakeDamage(damage, null, "Fireball");
            Debug.Log($"[FireballSkill] 일반 몬스터 {monster.name}에게 {damage} 데미지");
            damagedTargets.Add(monster);
        }
        else if (other.TryGetComponent(out BossStats boss) && !damagedTargets.Contains(boss))
        {
            boss.TakeDamage(damage);
            damagedTargets.Add(boss);
        }
    }
}