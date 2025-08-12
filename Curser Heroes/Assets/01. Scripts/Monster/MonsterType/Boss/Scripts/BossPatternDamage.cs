using UnityEngine;
//
public class BossPatternDamage : MonoBehaviour
{
    private Collider2D col;
    public float cooldown = 0.5f;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;        // 처음엔 꺼두기
    }
    public void Activate()
    {
        col.enabled = true;  // 패턴이 활성화 되면 콜라이더 활성 
        
    }
    public void Deactivate() // 패턴이 비활성화 되면 콜라이더 비활성
    {
        col.enabled = false;
    }

    public void Update()
    {
        if (col.enabled)
        {
            cooldown -= Time.deltaTime; // 쿨타임 감소
            if (cooldown <= 0f)
            {
                Attack(); // 쿨타임이 끝나면 비활성화
                cooldown = 0.5f; // 쿨타임 초기화
            }
        }
    }

    protected void Attack()
     {
        Bounds b = col.bounds;
        Collider2D weaponCollider = Physics2D.OverlapBox(b.center, b.size, 0f, LayerMask.GetMask("Weapon"));
        if (weaponCollider != null)
        {
            if (WeaponManager.Instance != null)
            {
                WeaponManager.Instance.TakeWeaponLifeDamage();
                Debug.Log("보스 공격으로 무기 내구도 감소!");
            }
            else
            {
                Debug.LogWarning("WeaponManager 인스턴스를 찾을 수 없습니다!");
            }
        }
    }
}
