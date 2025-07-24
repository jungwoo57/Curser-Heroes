using UnityEngine;

public class SpiderRed : BaseMonster
{
   public SpiderProjectile[] projectilePrefab; // 투사체 프리팹
    public Transform firePoint;         // 발사 위치
    public float attackRange = 5f;      // 공격 사거리
    public GameObject[] warningArea;
    [SerializeField] bool isAttacking = false;
    [SerializeField] private Vector2 targetPos;
    [SerializeField] private float minScale = 0.1f;
    [SerializeField] private float maxScale = 1.0f;
    [SerializeField] private float maxDistance;
    [SerializeField] private float speed =5.0f;
    [SerializeField] private float angle = 20.0f;
    
    protected override void Update()
    {
        base.Update();
        if (isAttacking)
        {
            WarningAreaChangeScale();
        }
        else
        {
            for (int i = 0; i < warningArea.Length; i++)
            {
                warningArea[i].gameObject.SetActive(false);
            }
        }

    }
    
    protected override void Attack()
    {
        //warningAreaChange 넣기
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider == null) return;
        
        targetPos = weaponCollider.transform.position; // 목표 지점 넣어주기
        maxDistance = Vector2.Distance(firePoint.transform.position, targetPos);
        
        SetWarningArea();
        
        Debug.Log("원거리 몬스터: 투사체 발사!");
    }

    private void SetWarningArea() // 3방향 공격 가능하게
    {
        isAttacking = true;
        //projectilePrefab[].transform.position = firePoint.position;
        //projectilePrefab.gameObject.SetActive(true);
        for (int i = 0; i < warningArea.Length; i++)
        {
            projectilePrefab[i].transform.position = firePoint.position;
            projectilePrefab[i].gameObject.SetActive(true);
            Vector2 dir = targetPos - (Vector2)firePoint.position;
            Vector2 newdir = Quaternion.Euler(0, 0, -angle + angle*i) * dir;
            Vector2 newPos = (Vector2)firePoint.position + newdir.normalized * maxDistance;
            
            warningArea[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); // 위험 범위 점점 커지게
            warningArea[i].transform.position = newPos;
            warningArea[i].gameObject.SetActive(true);
            projectilePrefab[i].Initialize(newPos, damage, speed);
        }
    }

    private void WarningAreaChangeScale() // 가까워 질수록 scale 커짐
    {
        for (int i = 0; i < warningArea.Length; i++)
        {
            float curDistance = Vector2.Distance(projectilePrefab[i].transform.position, targetPos);

            float progress = Mathf.Clamp01(1 - (curDistance / maxDistance));

            float scale = Mathf.Lerp(minScale, maxScale, progress);

            warningArea[i].transform.localScale = new Vector3(scale, scale, scale);

            if (warningArea[i].transform.localScale.x > 0.37f)
            {
                warningArea[i].gameObject.SetActive(false);
                isAttacking = false;
            }
        }
    }
    
    
}
