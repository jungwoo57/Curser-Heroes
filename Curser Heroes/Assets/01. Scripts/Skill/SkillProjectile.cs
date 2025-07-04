using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    private int damage;
    private float speed = 5f;
    private Vector2 direction;
    private float lifetime = 3f;
    private bool destroyOnHit = true;

    public void Init(int dmg, Vector2 dir = default, float spd = 5f, float life = 3f, bool destroy = true)
    {
        damage = dmg;
        direction = dir.normalized;
        speed = spd;
        lifetime = life;
        destroyOnHit = destroy;

        if (lifetime > 0 && destroyOnHit)
            Destroy(gameObject, lifetime); // 일정 시간 후 제거
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseMonster monster = other.GetComponent<BaseMonster>();
        if (monster != null)
        {
            monster.TakeDamage(damage);
            Debug.Log("매직소드가 적 타격함!");

            if (destroyOnHit)
                Destroy(gameObject);

            return; // 충돌 처리 종료
        }

        // 보스 몬스터 감지
        //BossBaseMonster boss = other.GetComponent<BossBaseMonster>();
        //if (boss != null)
        //{
        //    boss.TakeDamage(damage);

        //    if (destroyOnHit)
        //        Destroy(gameObject);
        //}
    }
}
