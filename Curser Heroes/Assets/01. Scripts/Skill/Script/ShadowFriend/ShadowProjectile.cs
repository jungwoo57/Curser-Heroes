using System.Collections;
using UnityEngine;

public class ShadowProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask monsterLayer;

    public float speed = 5f;
    public GameObject shadowFriendPrefab;

    private Vector2 moveDir;
    private SkillManager.SkillInstance skillInstance;
    private bool hasSpawned = false;
    private Animator animator;

    public void Init(Vector2 direction, SkillManager.SkillInstance instance)
    {
        moveDir = direction;
        skillInstance = instance;
        animator = GetComponent<Animator>();

        if (animator != null)
            animator.Play("Move");
    }

    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);

        // 화면 바깥 나가면 제거
        if (!IsVisibleFrom(Camera.main)) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasSpawned && ((1 << other.gameObject.layer) & monsterLayer) != 0)
        {
            hasSpawned = true;

            if (animator != null)
                animator.SetTrigger("doSpawn");

            StartCoroutine(SpawnShadowFriend());
        }
    }

    private IEnumerator SpawnShadowFriend()
    {
        yield return new WaitForSeconds(0.5f); // Spawn 애니메이션 길이만큼 대기
        GameObject friend = Instantiate(shadowFriendPrefab, transform.position, Quaternion.identity);
        friend.GetComponent<ShadowFriend>().Init(moveDir, skillInstance);

        Destroy(gameObject);
    }

    bool IsVisibleFrom(Camera cam)
    {
        Vector3 viewport = cam.WorldToViewportPoint(transform.position);
        return viewport.x > 0 && viewport.x < 1 && viewport.y > 0 && viewport.y < 1;
    }
}
