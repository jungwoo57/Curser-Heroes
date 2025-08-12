using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class LineProjectile : MonoBehaviour
{
    [Header("Line Settings")]
    public float length = 5f;
    public float width = 1f;
    public GameObject lineVFXPrefab;
    public float duration = 0.2f;
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
        // 1) VFX
        if (lineVFXPrefab != null)
        {
            var vfx = Instantiate(lineVFXPrefab, transform.position, transform.rotation);
            vfx.transform.localScale = new Vector3(width, length, 1f);
            Destroy(vfx, duration);
        }

        // 2) 데미지·상태이상 처리
        Vector3 boxCenter = transform.position + transform.up * (length / 2f);
        Vector2 boxSize = new Vector2(width, length);
        float angle = transform.eulerAngles.z;

        var hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle, monsterLayer);
        Debug.Log($"▶ LineProjectile hits: {hits.Length}");
        foreach (var col in hits)
        {
            if (col.TryGetComponent<BaseMonster>(out var m) && !m.IsDead)
            {
                m.TakeDamage(damageAmount, weaponData);

                switch (weaponData.effect)
                {
                    case SubWeaponEffect.Burn:
                        var burn = new BurnEffect(
                            m,
                            weaponData.burnDamagePerSecond,
                            weaponData.burnDuration,
                            1f
                        );
                        m.GetComponent<EffectManager>()?.AddEffect(burn);
                        break;

                    case SubWeaponEffect.Stun:
                        var stun = new StunEffect(weaponData.stunDuration);
                        m.GetComponent<EffectManager>()?.AddEffect(stun);
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
