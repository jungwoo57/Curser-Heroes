using UnityEngine;
using System.Collections.Generic;

public class ThornDome : MonoBehaviour
{
    [SerializeField] private LayerMask monsterLayerMask;

    private int damage;
    private Transform cursorTransform;
    private float duration = 5f;
    private HashSet<BaseMonster> damaged = new HashSet<BaseMonster>();

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    public void Init(int damage, Transform cursor)
    {
        this.damage = damage;
        this.cursorTransform = cursor;
        Debug.Log($"[ThornDome] Init: 데미지 {damage}");
        RotateTowardClosestMonster();
    }

    private void RotateTowardClosestMonster()
    {
        GameObject closest = FindClosestMonster();
        if (closest == null) return;

        Vector2 dir = (closest.transform.position - cursorTransform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Update()
    {
        if (cursorTransform != null)
        {
            transform.position = cursorTransform.position + transform.right * 0.3f;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log($"[ThornDome] 충돌 감지: {col.name}");
        if (col.TryGetComponent(out BaseMonster monster))
        {
            Debug.Log($"[ThornDome] 몬스터 충돌: {monster.name}");
            if (!damaged.Contains(monster))
            {
                Debug.Log($"[가시 돔] {monster.name}에게 {damage} 데미지 입힘");
                monster.TakeDamage(damage);
                damaged.Add(monster);
            }
        }
    }

    private GameObject FindClosestMonster()
    {
        float searchRadius = 10f; // 적절한 탐색 반경
        Collider2D[] hits = Physics2D.OverlapCircleAll(cursorTransform.position, searchRadius, monsterLayerMask);

        Debug.Log($"[ThornDome] 몬스터 레이어 탐색 결과: {hits.Length}개");

        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(cursorTransform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = hit.gameObject;
            }
        }

        return closest;
    }
}