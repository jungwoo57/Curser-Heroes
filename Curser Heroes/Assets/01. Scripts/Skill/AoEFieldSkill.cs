using UnityEngine;

public class AoEFieldSkill : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private LayerMask monsterLayer;  // 몬스터만 감지

    private Transform player;                         // 추적 대상 (플레이어)
    private float tickTimer;
    private SkillLevelData info;
    private float offsetRadius = 0.8f;

    // 초기화: 스킬 레벨 정보 + 플레이어 위치 받기
    public void Init(SkillManager.SkillInstance skillInstance, Transform playerTransform)
    {
        info = skillInstance.skill.levelDataList[skillInstance.level - 1];
        player = playerTransform;

        float diameter = offsetRadius * 2f * info.sizeMultiplier;
        float visualScaleMultiplier = 5f; // 원하는 시각적 크기 배율

        transform.localScale = Vector3.one * diameter * visualScaleMultiplier;
    }

    void Update()
    {
        if (player != null)
        {
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
        float radius = offsetRadius * info.sizeMultiplier;

        // 해당 레이어만 감지
        Collider2D[] monsters = Physics2D.OverlapCircleAll(transform.position, radius, monsterLayer);

        foreach (var col in monsters)
        {
            // 일반 몬스터 처리
            if (col.TryGetComponent(out BaseMonster baseMonster))
            {
                baseMonster.TakeDamage(info.damage);
                continue;
            }

            // 보스 몬스터 처리
            //if (col.TryGetComponent(out BossBaseMonster boss))
            //{
            //    boss.TakeDamage(info.damage);
            //}
        }
    }

    // 디버그용 범위 표시
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        float radius = offsetRadius * (info?.sizeMultiplier ?? 1f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}