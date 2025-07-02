using UnityEngine;

public class AoEFieldSkill : MonoBehaviour
{
    public Transform player;
    private float tickTimer;
    private SkillLevelData info;
    private float offsetRadius = 1.5f;

    public void Init(SkillManager.SkillInstance skillInstance)
    {
        info = skillInstance.skill.levelDataList[skillInstance.level - 1];
        player = GameObject.FindWithTag("Player").transform;
        transform.localScale = Vector3.one * info.sizeMultiplier;
    }

    void Update()
    {
        if (player != null)
        {
            // 플레이어 주위 고정
            transform.position = player.position;
        }

        tickTimer += Time.deltaTime;
        if (tickTimer >= 1f)
        {
            tickTimer = 0f;
            DealDamage();
        }
    }

    void DealDamage()
    {
        Collider2D[] monsters = Physics2D.OverlapCircleAll(transform.position, 1.5f * info.sizeMultiplier);
        foreach (var col in monsters)
        {
            BaseMonster monster = col.GetComponent<BaseMonster>();
            if (monster != null)
            {
                monster.TakeDamage(info.damage);
                continue;
            }

            BossBaseMonster boss = col.GetComponent<BossBaseMonster>();
            if (boss != null)
            {
                boss.TakeDamage(info.damage);
            }
        }
    }
}
