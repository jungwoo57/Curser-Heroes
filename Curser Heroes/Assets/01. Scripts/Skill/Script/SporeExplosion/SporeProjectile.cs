using UnityEngine;
using System.Collections.Generic;

public class SporeProjectile : MonoBehaviour
{
    private float speed;
    private int damage;
    private Vector2 direction;
    private HashSet<BaseMonster> damaged = new();

    public void Init(int damage, Vector2 dir, float speed)
    {
        this.damage = damage;
        this.direction = dir.normalized;
        this.speed = speed;
        Debug.Log($"[SporeProjectile] Init 호출됨: 데미지={damage}, 방향={direction}, 속도={speed}, 시작위치={transform.position}");
        Destroy(gameObject, 5f); // 수명 제한
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out BaseMonster monster))
        {
            if (damaged.Contains(monster))
            {
                Debug.Log($"[SporeProjectile] 이미 데미지를 입힌 몬스터: {monster.name}");
                return;
            }
            monster.TakeDamage(damage);
            damaged.Add(monster);
            Debug.Log($"[SporeProjectile] 몬스터 타격: {monster.name}, 데미지: {damage}");
        }
    }
}