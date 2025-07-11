using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    private int damage;
    private float speed = 5f;
    private Vector2 direction;

    public void Init(int dmg, Vector2 dir = default, float spd = 5f)
    {
        damage = dmg;
        direction = dir.normalized;
        speed = spd;
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

            return; // 충돌 처리 종료
        }

        // 보스 몬스터 감지
        BossStats boss = other.GetComponent<BossStats>();
        if (boss != null)
        {
            boss.TakeDamage(damage);

            return;
        }
    }
}
