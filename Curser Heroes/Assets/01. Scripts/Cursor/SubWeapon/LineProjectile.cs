using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class LineProjectile : MonoBehaviour
{
    [Header("Line Settings")]
    public float length = 5f;               // 앞쪽으로 뻗어나가는 길이
    public float width = 1f;                // 옆으로 퍼지는 폭
    public GameObject lineVFXPrefab;        // 길쭉한 VFX 프리팹
    public float duration = 0.2f;           // 지속 시간
    public LayerMask monsterLayer;

   
    private SubWeaponData weaponData;
    private int damageAmount;

    
    public void Init(SubWeaponData data, int damage)
    {
        weaponData = data;
        damageAmount = damage;
    }

    void Start()
    {
        //  시각 이펙트
        if (lineVFXPrefab != null)
        {
            var vfx = Instantiate(
                lineVFXPrefab,
                transform.position,
                transform.rotation
            );
            vfx.transform.localScale = new Vector3(width, length, 1f);
            Destroy(vfx, duration);
        }

        //  충돌 처리 및 데미지/상태이상 적용
        Vector3 boxCenter = transform.position + transform.up * (length / 2f);
        Vector2 boxSize = new Vector2(width, length);
        float angle = transform.eulerAngles.z;

        var hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle, monsterLayer);
        foreach (var col in hits)
        {
            if (col.TryGetComponent<BaseMonster>(out var m) && !m.IsDead)
            {
                // 기본 데미지
                m.TakeDamage(damageAmount, weaponData);

                // 상태이상 효과: Burn OR Stun
                switch (weaponData.effect)
                {
                    case SubWeaponEffect.Burn:
                        var burn = new BurnEffect();
                        burn.Apply(m);
                        m.GetComponent<EffectManager>()?.AddEffect(burn);
                        break;

                    case SubWeaponEffect.Stun:
                        m.Stun(weaponData.stunDuration);
                        break;
                }
            }
        }

        Destroy(gameObject, duration);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Matrix4x4 old = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawCube(Vector3.up * (length / 2f), new Vector3(width, length, 1f));
        Gizmos.matrix = old;
    }
#endif
}
