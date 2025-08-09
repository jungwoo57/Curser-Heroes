using UnityEngine;

public class MeteorSkill : MonoBehaviour
{
    private int damage;
    [SerializeField] private float delayBeforeHit = 0.5f; // 충돌 처리 전 대기 시간
    private float timer = 0f;
    [SerializeField] private Vector3 targetPosition;

    // A variable to hold the audio clip for the meteor
    private AudioClip meteorAudioClip;

    public void Init(int dmg, Vector3 targetPos, AudioClip clip)
    {
        damage = dmg;
        targetPosition = targetPos;
        meteorAudioClip = clip; // 💡 Init 메서드에서 오디오 클립을 할당합니다.

        // 시작 위치는 targetPosition의 오른쪽 위(사선방향), 예를 들어 y + 10f
        transform.position = new Vector3(targetPos.x, targetPos.y + 0.1f, targetPos.z);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= delayBeforeHit)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        if (meteorAudioClip != null) // 오디오 클립이 있는지 확인하고 재생합니다.
        {
            AudioSource.PlayClipAtPoint(meteorAudioClip, targetPosition);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(targetPosition, 0.5f, LayerMask.GetMask("Monster"));
        foreach (var col in hits)
        {
            if (col.TryGetComponent(out BaseMonster baseMonster))
                baseMonster.TakeDamage(damage);
            else if (col.TryGetComponent(out BossStats boss))
                boss.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetPosition, 0.5f);
    }
}