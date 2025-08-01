using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class SubProjectile : MonoBehaviour
{
    
    [SerializeField] protected SubWeaponData subWeaponData;
    [SerializeField] protected int calculatedDamage;
    protected BaseMonster target;
    protected float speed;

  
    public SubWeaponData WeaponData => subWeaponData;

    
    public int DamageAmount =>
        (calculatedDamage > 0)
            ? calculatedDamage
            : Mathf.RoundToInt(subWeaponData.GetDamage());

    public void Init(SubWeaponData data, BaseMonster tgt)
    {
        subWeaponData = data;
        target = tgt;
        speed = data.projectileSpeed;
        calculatedDamage = 0;
    }

    public void Init(SubWeaponData data, BaseMonster tgt, int damage)
    {
        subWeaponData = data;
        target = tgt;
        speed = data.projectileSpeed;
        calculatedDamage = damage;
    }

    private void Awake()
    {
        // Collider를 Trigger로 전환
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;

        // Rigidbody를 Kinematic으로 설정
        var rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.gravityScale = 0f;
    }

    protected virtual void Update()
    {
        if (target == null || target.IsDead)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }


    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<BaseMonster>(out var monster) && !monster.IsDead)
        {
            Debug.Log($"[SubProjectile] Hit {monster.name}, dmg = {DamageAmount}");
            monster.TakeDamage(DamageAmount, subWeaponData);

            // 상태이상 효과: Burn 또는 Stun 중 하나
            switch (subWeaponData.effect)
            {
                case SubWeaponEffect.Burn:
                    var burn = new BurnEffect(
                        monster,
                        subWeaponData.burnDamagePerSecond,
                        subWeaponData.burnDuration,
                        1f  // 1초마다 Tick
                    );
                    monster.GetComponent<EffectManager>()?.AddEffect(burn);
                    break;

                case SubWeaponEffect.Stun:
                    // StunEffect 생성자에 지속시간만 전달
                    var stun = new StunEffect(subWeaponData.stunDuration);
                    monster.GetComponent<EffectManager>()?.AddEffect(stun);
                    break;
            }

            Destroy(gameObject);
        }
    }


}
