using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ThornDome : MonoBehaviour
{
    [SerializeField] private LayerMask monsterLayerMask;

    private int damage;
    private Transform cursorTransform;
    private float duration = 5f;
    private HashSet<BaseMonster> damagedMonsters = new HashSet<BaseMonster>();
    private HashSet<BossStats> damagedBosses = new HashSet<BossStats>();

    private AudioSource audioSource;
    private AudioClip attackSound;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    public void Init(int damage, Transform cursor, AudioClip clip)
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        this.attackSound = clip;

        this.damage = damage;
        this.cursorTransform = cursor;
        Debug.Log($"[ThornDome] Init: 데미지 {damage}");
        RotateTowardClosestMonster();

        transform.position = cursorTransform.position + transform.up * 0.3f;
    }

    private void RotateTowardClosestMonster()
    {
        GameObject closest = FindClosestMonster();
        if (closest == null) return;

        Vector2 dir = (closest.transform.position - cursorTransform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle -90);
    }

    void Update()
    {
        if (cursorTransform != null)
        {
            transform.position = cursorTransform.position + transform.up * 0.3f;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log($"[ThornDome] 충돌 감지: {col.name}");
        
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        if (col.TryGetComponent<BaseMonster>(out var monster))
        {
            Debug.Log($"[ThornDome] 몬스터 충돌: {monster.name}");
            if (!damagedMonsters.Contains(monster))
            {
                Debug.Log($"[가시 돔] {monster.name}에게 {damage} 데미지 입힘");
                monster.TakeDamage(damage);
                damagedMonsters.Add(monster);
            }
            return;  // 몬스터 처리했으면 보스 체크 안 함
        }

        if (col.TryGetComponent<BossStats>(out var boss))
        {
            Debug.Log($"[ThornDome] 보스 충돌: {boss.name}");
            if (!damagedBosses.Contains(boss))
            {
                Debug.Log($"[가시 돔] 보스 {boss.name}에게 {damage} 데미지 입힘");
                boss.TakeDamage(damage);
                damagedBosses.Add(boss);
            }
        }
    }

    private GameObject FindClosestMonster()
    {
        float searchRadius = 10f; // 적절한 탐색 반경
        Collider2D[] hits = Physics2D.OverlapCircleAll(cursorTransform.position, searchRadius, monsterLayerMask);

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