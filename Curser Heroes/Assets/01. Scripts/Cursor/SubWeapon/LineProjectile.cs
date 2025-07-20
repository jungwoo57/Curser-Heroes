using UnityEngine;

public class LineProjectile : MonoBehaviour
{
    [Header("Line Settings")]
    public float length = 5f;               // 앞쪽으로 뻗어나가는 길이
    public float width = 1f;               // 옆으로 퍼지는 폭
    public int damage = 5;
    public LayerMask monsterLayer;
    public GameObject lineVFXPrefab;        // 길쭉한 VFX 프리팹
    public float duration = 0.2f;           // 지속 시간

    [Header("Stun Settings")]
    [Tooltip("명중 시 스턴을 적용할지")]
    public bool applyStun = false;
    [Tooltip("스턴 지속 시간(초)")]
    public float stunDuration = 1f;

    void Start()
    {
        //시각 효과
        if (lineVFXPrefab != null)
        {
            var vfx = Instantiate(lineVFXPrefab, transform.position, transform.rotation);
            vfx.transform.localScale = new Vector3(width, length, 1f);
            Destroy(vfx, duration);
        }

        //스턴 처리
        Vector3 boxCenter = transform.position + transform.up * (length / 2f);
        Vector2 boxSize = new Vector2(width, length);
        float angle = transform.eulerAngles.z;

        var hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle, monsterLayer);
        Debug.Log($"▶ LineProjectile hits: {hits.Length}");
        foreach (var col in hits)
        {
            if (col.TryGetComponent<BaseMonster>(out var m) && !m.IsDead)
            {
                
                m.TakeDamage(damage);

                // 스턴 Inspector 옵션
                if (applyStun && stunDuration > 0f)
                {
                    var em = m.GetComponent<EffectManager>();
                    //if (em != null) 
                        //em.AddEffect(new StunEffect(m, stunDuration));
                }
            }
        }

       
        Destroy(gameObject, duration);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Matrix4x4 old = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawCube(Vector3.up * (length / 2f), new Vector3(width, length, 1f));
        Gizmos.matrix = old;
    }
#endif
}
