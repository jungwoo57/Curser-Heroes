using UnityEngine;

public class SubAreaAttack : MonoBehaviour
{
    public float duration = 0.2f;


    public void Init(SubWeaponData data)
    {
        SubWeaponData weaponData = data;
    }
        void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(10); // 나중에 SubWeaponData에서 damage로 수정
                // 효과 적용은 추후 연결
            }
        }
    }
}
