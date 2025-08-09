using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    private int damage;
    private float speed = 5f;
    private Vector2 direction;

    private AudioSource audioSource;
    public AudioClip attackSound;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
            // 💡 몬스터와 충돌 시 공격음 재생
            if (audioSource != null && attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
            monster.TakeDamage(damage);

            return;
        }

        BossStats boss = other.GetComponent<BossStats>();
        if (boss != null)
        {
            // 💡 보스와 충돌 시 공격음 재생
            if (audioSource != null && attackSound != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
            boss.TakeDamage(damage);

            return;
        }
    }
}
