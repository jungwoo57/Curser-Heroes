using UnityEngine;

public class MonsterProjectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 direction;
    private int damage;

    public void Initialize(Vector3 dir, int dmg)
    {
        direction = dir;
        damage = dmg;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            // 싱글톤을 통해 무기 내구도 감소
            if (WeaponManager.Instance != null)
            {
                WeaponManager.Instance.TakeWeaponLifeDamage();
                Debug.Log("투사체 충돌: 무기 내구도 감소!");
            }

            Destroy(gameObject); // 투사체 제거
        }
    }
}
