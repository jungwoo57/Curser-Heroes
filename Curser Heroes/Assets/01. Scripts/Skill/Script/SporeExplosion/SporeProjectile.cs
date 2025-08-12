using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SporeProjectile : MonoBehaviour
{
    [SerializeField]private float speed;
    private int damage;
    [SerializeField]private Vector2 direction;
    private HashSet<BaseMonster> damagedMonsters = new HashSet<BaseMonster>();
    private HashSet<BossStats> damagedBosses = new HashSet<BossStats>();

    private AudioClip hitAudioClip;
    private AudioSource audioSource;

    public void Init(int damage, Vector2 dir, float speed, AudioClip audioClip)
    {
        this.damage = damage;
        this.direction = dir.normalized;
        this.speed = speed;
        // ⭐ 오디오 클립 초기화
        this.hitAudioClip = audioClip;

        // ⭐ AudioSource 컴포넌트 가져오기 또는 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        Debug.Log($"[SporeProjectile] Init 호출됨: 데미지={damage}, 방향={direction}, 속도={speed}, 시작위치={transform.position}");
        Destroy(gameObject, 5f); // 수명 제한
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<BaseMonster>(out var monster))
        {
            if (damagedMonsters.Contains(monster))
            {
                Debug.Log($"[SporeProjectile] 이미 데미지를 입힌 몬스터: {monster.name}");
                return;
            }
            monster.TakeDamage(damage);
            damagedMonsters.Add(monster);

            if (audioSource != null && hitAudioClip != null)
            {
                audioSource.PlayOneShot(hitAudioClip);
            }

            Debug.Log($"[SporeProjectile] 몬스터 타격: {monster.name}, 데미지: {damage}");
            return;  // 몬스터 처리 완료
        }

        if (col.TryGetComponent<BossStats>(out var boss))
        {
            if (damagedBosses.Contains(boss))
            {
                Debug.Log($"[SporeProjectile] 이미 데미지를 입힌 보스: {boss.name}");
                return;
            }
            boss.TakeDamage(damage);
            damagedBosses.Add(boss);

            if (audioSource != null && hitAudioClip != null)
            {
                audioSource.PlayOneShot(hitAudioClip);
            }

            Debug.Log($"[SporeProjectile] 보스 타격: {boss.name}, 데미지: {damage}");
        }
    }
}