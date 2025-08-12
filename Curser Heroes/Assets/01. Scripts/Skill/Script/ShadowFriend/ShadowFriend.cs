using UnityEngine;

public class ShadowFriend : MonoBehaviour
{
    [SerializeField] private LayerMask monsterLayer;
    [SerializeField] private LayerMask projectileLayer;

    public float speed = 1f;
    public float lifetime = 4f;
    private float pushPower = 3f;

    private Vector2 moveDir;
    private SkillManager.SkillInstance skillInstance;

    public void Init(Vector2 direction, SkillManager.SkillInstance instance)
    {
        moveDir = direction;
        skillInstance = instance;
        // Idle 애니메이션 루프
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & monsterLayer) != 0)
        {
            // 몬스터를 밀어낼 방향과 힘 계산
            Vector2 pushDir = (other.transform.position - transform.position).normalized;
            Vector3 newPosition = other.transform.position + (Vector3)(pushDir * pushPower * Time.deltaTime);

            // X축과 Y축 위치를 제한
            newPosition.x = Mathf.Clamp(newPosition.x, -8f, 8f);
            newPosition.y = Mathf.Clamp(newPosition.y, -2.5f, 2f);

            // 제한된 위치로 몬스터의 위치 업데이트
            other.transform.position = newPosition;
        }
        else if (((1 << other.gameObject.layer) & projectileLayer) != 0)
        {
            Destroy(other.gameObject);
        }
    }
}
