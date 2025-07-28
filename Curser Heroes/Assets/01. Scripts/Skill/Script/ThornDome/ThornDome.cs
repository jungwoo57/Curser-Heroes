using UnityEngine;
using System.Collections.Generic;

public class ThornDome : MonoBehaviour
{
    [SerializeField] private LayerMask monsterLayerMask;

    private int damage;
    private Transform cursorTransform;
    private Vector2 direction;
    private bool initialized = false;
    private float duration = 5f;
    private HashSet<BaseMonster> damaged = new HashSet<BaseMonster>();

    public void Init(int damage, Transform cursor, Vector2 dir)
    {
        this.damage = damage;
        this.cursorTransform = cursor;
        this.direction = dir.normalized;

        // 회전 방향 설정
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        initialized = true;

        Destroy(gameObject, duration);
    }

    private void Update()
    {
        if (!initialized || cursorTransform == null)
            return;

        // 커서 기준 direction 방향으로 0.3f 거리 고정
        transform.position = cursorTransform.position + (Vector3)(direction * 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out BaseMonster monster))
        {
            if (!damaged.Contains(monster))
            {
                monster.TakeDamage(damage);
                damaged.Add(monster);
            }
        }
    }
}