using UnityEngine;

public class RadialProjectile : MonoBehaviour
{
    [Header("데이터 (프리팹 인스펙터에서 할당)")]
    [Tooltip("이 투사체가 사용할 SubWeaponData")]
    [SerializeField] private SubWeaponData subWeaponData;

    [Header("데미지 (Manager가 Assign)")]
    [Tooltip("Manager가 Instantiate 후 설정해 주거나, 인스펙터에서 기본값 지정 가능")]
    public int damage = 0;

    [Header("레이어 마스크 (프리팹 인스펙터에서 할당)")]
    public LayerMask monsterLayer;

    [Header("VFX (프리팹 인스펙터에서 할당)")]
    public GameObject vfxPrefab;

    [Header("범위 & 스턴 (SubWeaponData 대신 Inspector에서 덮어쓰기 가능)")]
    public float radius = 3f;
    public bool applyStun = false;
    public float stunDuration = 1f;

    void Start()
    {
        Debug.Log($"[RadialProjectile] radius={radius}, vfxPrefab={vfxPrefab?.name}");
        // 만약 인스펙터에 SubWeaponData를 할당했다면 기본값 채워주기
        if (subWeaponData != null)
        {
            radius = subWeaponData.effectRadius;
            applyStun = subWeaponData.stunOnRadial;
            stunDuration = subWeaponData.stunDuration;
            if (damage == 0)
                damage = Mathf.RoundToInt(subWeaponData.GetDamage());
            if (vfxPrefab == null)
                vfxPrefab = subWeaponData.ForceVisualPrefab;
        }

        Explode();
    }

    private void Explode()
    {
        // 1) 범위 내 몬스터 검색
        var hits = Physics2D.OverlapCircleAll(transform.position, radius, monsterLayer);
        foreach (var hit in hits)
        {
            var m = hit.GetComponent<BaseMonster>();
            if (m == null || m.IsDead) continue;
            m.TakeDamage(damage, subWeaponData);
            if (applyStun) m.Stun(stunDuration);
        }

        // 2) VFX
        if (vfxPrefab != null)
        {
            var fx = Instantiate(vfxPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 1f);
        }

        Destroy(gameObject);
    }

    // (디버그용)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
