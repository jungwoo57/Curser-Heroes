using UnityEngine;

public class SubWeaponProjectile : MonoBehaviour
{
    public float speed = 10f;         // 투사체 속도
    public float lifeTime = 2f;       // 몇 초 후에 사라짐

    private float damage;             // 데미지 저장용
    private ISubWeaponEffect effect;  // 무기 효과 저장 

    // 투사체 발사 준비 (외부에서 호출)
    public void Init(float _damage, ISubWeaponEffect _effect, Vector2 direction)
    {
        damage = _damage;
        effect = _effect;

        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();           // Rigidbody2D를 이용해서 방향으로 날아감
        rb.velocity = direction.normalized * speed;

        
        Destroy(gameObject, lifeTime);          // 일정 시간 뒤에 자동으로 삭제됨
    }

   
    private void OnTriggerEnter2D(Collider2D other)              // 몬스터와 부딪혔을 때
    {
        BaseMonster monster = other.GetComponent<BaseMonster>();

       
        if (monster != null && effect != null)           // 몬스터가 맞았고, 효과가 있으면 실행
        {
            effect.ApplyEffect(monster, damage);
            Destroy(gameObject);            // 맞추고 나면 사라짐
        }
    }
}
