using Unity.VisualScripting;
using UnityEngine;

public class Bivalvia : BaseMonster
{
    public BivalviaProjectile projectilePrefab; // 투사체 프리팹
    public Transform firePoint;         // 발사 위치
    public float attackRange = 5f;      // 공격 사거리
    public GameObject warningArea;
    [SerializeField] bool isAttacking = false;
    [SerializeField] private Vector2 targetPos;
    [SerializeField] private float minScale = 0.1f;
    [SerializeField] private float maxScale = 1.0f;
    [SerializeField] private float maxDistance;
    [SerializeField] private float speed =5.0f;


    protected override void Start()
    {
        projectilePrefab.gameObject.SetActive(false);
    }
    protected override void Update()
    {
        base.Update();
        if (isAttacking)
        {
            WarningAreaChangeScale();
        }
    }
    
    protected override void Attack()
    {
        //warningAreaChange 넣기
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider == null) return;
        
        targetPos = weaponCollider.transform.position; // 목표 지점 넣어주기
        maxDistance = Vector2.Distance(projectilePrefab.transform.position, targetPos);
        
        SetWarningArea();
        
        projectilePrefab.Initialize(targetPos, damage, speed);
        /*
        if (projectilePrefab != null && firePoint != null)
        {
            Vector3 direction = (weaponCollider.transform.position - firePoint.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            MonsterProjectile projScript = projectile.GetComponent<MonsterProjectile>();
            if (projScript != null)
                projScript.Initialize(direction, damage);
        }
        같은 스크립트를 적용할지 추후 생각*/ 
    }

    private void SetWarningArea()
    {
        isAttacking = true;
        projectilePrefab.transform.position = firePoint.position;
        projectilePrefab.gameObject.SetActive(true);
        warningArea.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); // 위험 범위 점점 커지게
        warningArea.transform.position = targetPos;
        warningArea.gameObject.SetActive(true);
    }

    private void WarningAreaChangeScale() // 가까워 질수록 scale 커짐
    {
        float curDistance = Vector2.Distance(projectilePrefab.transform.position, targetPos);
        
        float progress = Mathf.Clamp01(1 - (curDistance / maxDistance));
        
        float scale = Mathf.Lerp(minScale, maxScale, progress);
        
        warningArea.transform.localScale = new Vector3(scale, scale, scale);

        if (warningArea.transform.localScale.x > 0.35f)
        {
            warningArea.gameObject.SetActive(false);
            isAttacking = false;
        }
    }
}
