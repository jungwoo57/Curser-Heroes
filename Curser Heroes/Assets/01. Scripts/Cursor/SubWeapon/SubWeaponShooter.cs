using UnityEngine;

public class SubWeaponShooter : MonoBehaviour
{
    public SubWeaponData currentSubWeapon;
    public SubWeaponUpgrade subWeaponUpgrade;
    public Transform shootPoint;

    private float cooldownTimer = 0f;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0f)
        {
            Fire();
            cooldownTimer = currentSubWeapon.cooldown;
        }
    }

    void Fire()
    {
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);      // 마우스 위치를 월드 좌표로 바꿈
        mousePos.z = 0f;          // z 좌표는 0으로 설정
        Vector2 direction = (mousePos - shootPoint.position).normalized;       //마우스 방향으로 설정
        GameObject bullet = Instantiate(currentSubWeapon.projectilePrefab, shootPoint.position, Quaternion.identity);   //투사체 생성       
        ISubWeaponEffect effect = bullet.GetComponent<ISubWeaponEffect>();             // 효과 불러오기      
        SubWeaponProjectile bulletScript = bullet.GetComponent<SubWeaponProjectile>();  // 투사체 정보 가져오기

        // 투사체 초기화 (데미지, 효과, 날아갈 방향)
        float damage = currentSubWeapon.GetDamage(subWeaponUpgrade.weaponLevel); // 무기 데이터에서 데미지 가져오기
        bulletScript.Init(damage, effect, direction);
    }

}

