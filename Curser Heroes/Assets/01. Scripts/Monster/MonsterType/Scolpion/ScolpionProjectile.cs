using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScolpionProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 direction;
    private int damage;
    [SerializeField] private float durationTime;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private Animator animator;
    
    public void Initialize(Vector3 pos, int dmg, float speed)
    {
        targetPos = pos;
        direction = (pos - transform.position).normalized;
        damage = dmg;
        this.speed = speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle+90f);
    }

    void Update()
    {
        StartCoroutine(DisAbleTime());
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
                Debug.Log("투사체 충돌: 무기 내구도 감소!");
                Destroy(gameObject);
                // 즉시 파괴, 충돌했을 때만
            }
            
        }
    }
    
    IEnumerator DisAbleTime()
    {
        yield return new WaitForSeconds(durationTime);
        Destroy(gameObject);
    }
}
