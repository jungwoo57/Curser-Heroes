using UnityEngine;
using System.Collections.Generic;

public class ThornDome : MonoBehaviour
{
    private int damage;
    private Transform cursorTransform;
    private float duration = 5f;
    private HashSet<BaseMonster> damaged = new();

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    public void Init(int damage, Transform cursor)
    {
        this.damage = damage;
        this.cursorTransform = cursor;

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
        if (col.TryGetComponent(out BaseMonster monster))
        {
            if (!damaged.Contains(monster))
            {
                monster.TakeDamage(damage);
                damaged.Add(monster);
            }
        }
    }

    private GameObject FindClosestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (var m in monsters)
        {
            float dist = Vector2.Distance(cursorTransform.position, m.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = m;
            }
        }

        return closest;
    }
}