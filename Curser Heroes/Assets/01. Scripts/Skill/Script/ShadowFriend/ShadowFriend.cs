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
            Vector2 pushDir = (other.transform.position - transform.position).normalized;
            other.transform.position += (Vector3)(pushDir * pushPower * Time.deltaTime);
        }
        else if (((1 << other.gameObject.layer) & projectileLayer) != 0)
        {
            Destroy(other.gameObject);
        }
    }
}
