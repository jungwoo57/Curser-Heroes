using UnityEngine;

public class AoEFieldSkill : MonoBehaviour

{

    [Header("설정")]

    [SerializeField] private LayerMask monsterLayer;  // 몬스터만 감지



    private Transform player;                         // 추적 대상 (플레이어)

    private float tickTimer;

    private SkillLevelData info;

    private float offsetRadius = 0.75f;



    private AudioSource audioSource;

    private SkillManager.SkillInstance skillInstance;



    // 초기화: 스킬 레벨 정보 + 플레이어 위치 받기

    public void Init(SkillManager.SkillInstance skillInstance, Transform playerTransform)

    {

        this.skillInstance = skillInstance; // 초기화

        info = skillInstance.skill.levelDataList[skillInstance.level - 1];

        player = playerTransform;



        // ⭐ AudioSource 컴포넌트 가져오기

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {

            audioSource = gameObject.AddComponent<AudioSource>();

        }



        info = skillInstance.skill.levelDataList[skillInstance.level - 1];

        player = playerTransform;



        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr == null || sr.sprite == null)

        {

            Debug.LogWarning("SpriteRenderer 또는 Sprite가 없습니다.");

            return;

        }



        float baseSpriteDiameter = sr.sprite.bounds.size.x;

        transform.localScale = Vector3.one * offsetRadius * 2f * info.sizeMultiplier / baseSpriteDiameter;

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



        if (audioSource != null && skillInstance.skill.audioClip != null)

        {

            audioSource.PlayOneShot(skillInstance.skill.audioClip);

        }



        foreach (var col in monsters)

        {

            // 일반 몬스터 처리

            if (col.TryGetComponent(out BaseMonster baseMonster))

            {

                baseMonster.TakeDamage(info.damage);

                continue;

            }



            // 보스 몬스터 처리

            if (col.TryGetComponent(out BossStats boss))

            {

                boss.TakeDamage(info.damage);

            }

        }

    }



    // 디버그용 범위 표시

    void OnDrawGizmosSelected()

    {

        Gizmos.color = Color.magenta;

        float radius = offsetRadius * (info?.sizeMultiplier ?? 1f);

        Gizmos.DrawWireSphere(transform.position, radius);

    }





}