using UnityEngine;

public class RadialProjectile : MonoBehaviour
{
    [Header("포스 범위")]
    [SerializeField, Range(0.1f, 10f)]
    private float forceRadius = 3f;    // Inspector 슬라이더로만 제어

    private int damage;
    private LayerMask monsterLayer;
    private GameObject vfxPrefab;
    private float duration = 0.6f;

 
    public void Init(int dmg, LayerMask layer, GameObject vfx)
    {
        damage = dmg;
        monsterLayer = layer;
        vfxPrefab = vfx;
        // forceRadius는 Inspector 값 그대로 사용
    }

    void Start()
    {
        // 1) VFX
        if (vfxPrefab != null)
        {
            var vfx = Instantiate(vfxPrefab, transform.position, Quaternion.identity);
            vfx.transform.localScale = Vector3.one * forceRadius * 2f;
            Destroy(vfx, duration);
        }

        // 2) 데미지
        var hits = Physics2D.OverlapCircleAll(transform.position, forceRadius, monsterLayer);
        Debug.Log($"▶ Hits: {hits.Length}");
        foreach (var col in hits)
            if (col.TryGetComponent<BaseMonster>(out var m) && !m.IsDead)
                m.TakeDamage(damage);

        // 3) 정리
        Destroy(gameObject, duration);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, forceRadius);
    }
#endif
}
