using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BivalviaProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 direction;
    private int damage;
    [SerializeField] private float durationTime;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D col;
    [SerializeField] private float stunDuration;
    public void Initialize(Vector3 pos, int dmg, float speed)
    {
        col = GetComponent<Collider2D>();
        targetPos = pos;
        direction = (pos - transform.position).normalized;
        damage = dmg;
        this.speed = speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle+90f);
        col.enabled = false;
    }

    void Update()
    {
        ArrivePos();
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            // 무기 내구도 감소
            if (WeaponManager.Instance != null && !WeaponManager.Instance.isInvincible)
            {
                WeaponManager.Instance.TakeWeaponLifeDamage();
                if (other.GetComponent<CursorMoving>())
                {
                    CursorMoving cursor = other.GetComponent<CursorMoving>();
                    cursor.Stun(stunDuration);
                    Debug.Log("스턴 적용됨");
                }
                Debug.Log("투사체 충돌: 무기 내구도 감소!");
                // 즉시 파괴, 충돌했을 때만
            }
            
        }
    }

    private void ArrivePos()
    {
        if (Vector2.Distance(transform.position, targetPos) >= 0.1f)
            return;
        
        if (animator != null)
        {
            animator.SetBool("Atk", true);
        }

        StartCoroutine(Arrive());
    }

    IEnumerator Arrive()
    {
        speed = 0;
        col.enabled = true;
        yield return new WaitForSeconds(durationTime);
        col.enabled = false;
        gameObject.SetActive(false);
    }
}
