using UnityEngine;

public class MirrorCursor : MonoBehaviour
{
    [SerializeField] private LayerMask monsterLayer;

    private Transform targetCursor;
    private float damage;
    private float duration;
    private float timer;
    private Vector3 offset;

    public void Init(Transform originalCursor, float damage, float duration, Vector3 offset)
    {
        this.targetCursor = originalCursor;
        this.damage = damage;
        this.duration = duration;
        this.offset = offset;

        // 외형 설정
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = originalCursor.GetComponent<SpriteRenderer>().sprite;
        sr.color = new Color(1, 1, 1, 120f / 255f); // 알파값

        // 충돌 안 받도록 설정
        gameObject.layer = LayerMask.NameToLayer("Default");
        Collider2D col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject);
            return;
        }

        // 커서 따라다니기
        if (targetCursor != null)
        {
            transform.position = targetCursor.position + offset;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[MirrorCursor] 충돌 감지: {other.name}");

        if (((1 << other.gameObject.layer) & monsterLayer) == 0)
            return;

        // BaseMonster 처리
        if (other.TryGetComponent<BaseMonster>(out var baseMonster))
        {
            baseMonster.TakeDamage(Mathf.Max(1, Mathf.RoundToInt(damage)));
            Debug.Log($"[MirrorCursor] {baseMonster.name}에게 {damage} 데미지 입힘");
            return;
        }

        // BossStats 처리
        if (other.TryGetComponent<BossStats>(out var boss))
        {
            boss.TakeDamage(Mathf.Max(1, Mathf.RoundToInt(damage)));
            Debug.Log($"[MirrorCursor] 보스 {boss.name}에게 {damage} 데미지 입힘");
        }
    }
}