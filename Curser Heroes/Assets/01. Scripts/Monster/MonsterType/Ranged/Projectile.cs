using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;    // 투사체 이동 속도
    private Vector3 moveDirection;   // 이동 방향 (단위 벡터)
    private int damage;              // 투사체가 입힐 데미지

    // 투사체 초기화 함수 (발사 방향, 데미지 설정)
    public void Initialize(Vector3 direction, int damageAmount)
    {
        moveDirection = direction.normalized;
        damage = damageAmount;
    }

    // 매 프레임 이동 처리
    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    // 충돌 감지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 대상에 WeaponManager 컴포넌트가 있으면 (무기 맞음)
        WeaponManager weaponManager = collision.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            weaponManager.TakeWeaponLifeDamage();  // 무기 내구도 감소
            Destroy(gameObject);                    // 투사체 파괴
            return;
        }

        // 장애물이나 벽 등과 충돌하면 투사체 파괴
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
