using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillManager;

public class RotatingSkill : MonoBehaviour
{
    public GameObject rotatingObjectPrefab;

    private Transform player;
    private float rotateSpeed = 90f;
    private float radius = 1f;

    private AudioSource audioSource;
    private int currentDamage;
    private float damageCooldown = 1.5f;
    private SkillManager.SkillInstance skillInstance;

    public void Init(SkillManager.SkillInstance skillInstance, Transform playerTransform)
    {
        this.skillInstance = skillInstance;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (audioSource != null && skillInstance.skill.audioClip != null)
        {
            audioSource.PlayOneShot(skillInstance.skill.audioClip);
        }

        var levelData = skillInstance.skill.levelDataList[skillInstance.level - 1];
        player = playerTransform;
        currentDamage = levelData.damage;

        int count = levelData.count;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            Quaternion rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg - 90);
            GameObject obj = Instantiate(rotatingObjectPrefab, transform.position + offset, rotation, transform);
            obj.transform.localScale *= levelData.sizeMultiplier;

            if (obj.TryGetComponent(out SkillProjectile proj))
            {
                // SkillProjectile에 부모 스크립트의 레퍼런스를 전달합니다.
                proj.Init(this, currentDamage);
            }
        }
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position;
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        }
    }

    public void UpdateSwords(SkillManager.SkillInstance skillInstance)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Init(skillInstance, player);
    }

    // 이 메서드는 이제 충돌 무시 로직을 포함합니다.
    public void ApplyDamageAndDisableCollision(Collider2D monsterCollider)
    {
        BaseMonster monster = monsterCollider.GetComponent<BaseMonster>();
        BossStats boss = monsterCollider.GetComponent<BossStats>();

        // 몬스터에게 데미지 적용
        if (monster != null)
        {
            // 몬스터 피격 시 사운드 재생
            if (audioSource != null && skillInstance.skill.audioClip != null)
            {
                audioSource.PlayOneShot(skillInstance.skill.audioClip);
            }
            monster.TakeDamage(currentDamage);
        }
        else if (boss != null)
        {
            // 보스 피격 시 사운드 재생
            if (audioSource != null && skillInstance.skill.audioClip != null)
            {
                audioSource.PlayOneShot(skillInstance.skill.audioClip);
            }
            boss.TakeDamage(currentDamage);
        }

        // 모든 자식 칼날들과 몬스터 간의 충돌을 1.5초 동안 무시합니다.
        StartCoroutine(IgnoreCollisions(monsterCollider));
    }

    private System.Collections.IEnumerator IgnoreCollisions(Collider2D monsterCollider)
    {
        // 몬스터 콜라이더가 이미 파괴되었거나 비활성화된 경우, 코루틴을 즉시 종료
        if (monsterCollider == null || !monsterCollider.gameObject.activeInHierarchy)
        {
            yield break;
        }

        List<Collider2D> swordColliders = new List<Collider2D>();
        foreach (Transform child in transform)
        {
            Collider2D swordCollider = child.GetComponent<Collider2D>();
            if (swordCollider != null)
            {
                swordColliders.Add(swordCollider);
            }
        }

        // 몬스터와 모든 칼날 간의 충돌 무시
        foreach (Collider2D swordCollider in swordColliders)
        {
            // 칼날이나 몬스터가 사라졌을 경우를 대비하여 null 체크를 추가
            if (swordCollider != null && monsterCollider != null)
            {
                Physics2D.IgnoreCollision(swordCollider, monsterCollider, true);
            }
        }

        // 1.5초 대기
        yield return new WaitForSeconds(damageCooldown);

        // 다시 충돌 감지 활성화
        foreach (Collider2D swordCollider in swordColliders)
        {
            // 칼날이나 몬스터가 사라졌을 경우를 대비하여 null 체크를 추가
            if (swordCollider != null && monsterCollider != null)
            {
                Physics2D.IgnoreCollision(swordCollider, monsterCollider, false);
            }
        }
    }
}