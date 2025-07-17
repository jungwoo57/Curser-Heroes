using UnityEngine;

public class RabbitGirlEffect : MonoBehaviour
{
    public GameObject impactEffect;
    public Transform carrot;

    private Animator animator;
    private Vector3 dropTarget;

    private bool isDropping = false;
    private float dropDuration = 0.4f;
    private float dropTimer = 0f;

    private bool hasDropped = false; // 중복 방지용

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartThrow(Vector3 center, Vector3 cursor)
    {
        Debug.Log("[RabbitGirlEffect] StartThrow 호출됨");
        dropTarget = cursor;
        animator.Play("ThrowToCenter");
        hasDropped = false;
    }

    public void OnThrowToCenterEnd()
    {
        Debug.Log("[RabbitGirlEffect] OnThrowToCenterEnd 호출됨");
        if (hasDropped) return;

        animator.SetTrigger("dropTrigger");
        isDropping = true;
        dropTimer = 0f;
    }

    void Update()
    {
        Debug.Log($"[RabbitGirlEffect] Update 호출 중 - {Time.time}");
        // 애니메이션 상태 디버그
        if (animator != null)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        }

        if (isDropping)
        {
            dropTimer += Time.deltaTime;
            float t = Mathf.Clamp01(dropTimer / dropDuration);
            carrot.position = Vector3.Lerp(transform.position, dropTarget, t);

            if (t >= 1f)
            {
                isDropping = false;
                OnDropToCursorEnd();
            }
        }
    }

    public void OnDropToCursorEnd()
    {
        if (hasDropped) return;
        hasDropped = true;

        Debug.Log("[RabbitGirlEffect] OnDropToCursorEnd 호출, 오브젝트 파괴 시작");

        if (impactEffect != null)
            Instantiate(impactEffect, dropTarget, Quaternion.identity);
        Debug.Log($"[RabbitGirlEffect] Destroy 호출 전 carrot 위치: {carrot.position}");
        Destroy(gameObject);
        Debug.Log("[RabbitGirlEffect] Destroy 호출 완료");
    }
}