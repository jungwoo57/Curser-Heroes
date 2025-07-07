using System;
using UnityEngine;


public class SubWeaponManager : MonoBehaviour
{
    public SubWeaponData equippedSubWeapon;    //현재 장착중인 보조무기 데이터
    [SerializeField]private float currentCooldown = 0f;      //현재 쿨타임 남은시간
    public LayerMask monsterLayer;
    void Update()
    {
        if (currentCooldown > 0f)
            currentCooldown -= Time.deltaTime;     //쿨타임이 남아있다면 프레임마다 감소

        if (Input.GetMouseButtonDown(0) && CanUseSubWeapon())
        {
            UseSubWeapon();
        }              //마우스 좌클릭시 보조무기를 사용할 수 있는지 체크하고 사용
    }

    public bool CanUseSubWeapon()
    {
        return equippedSubWeapon != null && currentCooldown <= 0f;     //보조무기 사용할 수 있는지 확인(장착 확인 절차)
    }

    public void UseSubWeapon()
    {
        currentCooldown = equippedSubWeapon.cooldown;

        if ((SubWeaponRangeType)equippedSubWeapon.rangeType == SubWeaponRangeType.Radial)
        {
            ShootAreaAroundCursor(); // 범위형 타입을 가진 보조무기라면 커서 주변으로 데미지를 주는 데미지존 효과 설정 
        }
        else
        {
            ShootToNearestEnemy(); // 위에 경우를 제외한 나머지는 자동조준
        }
    }

    void ShootToNearestEnemy()   //자동조준 발사
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);   //메인카메라에서 커서의 좌표
        cursorPos.z = 0f;

        BaseMonster target = FindNearestAliveMonster(cursorPos);    //커서 주변에서 가장 가까운 몬스터 탐색
        if (target == null)
        {
            Debug.LogWarning("타겟 몬스터 없음");
            return;
        }

       

        GameObject proj = Instantiate(equippedSubWeapon.projectilePrefab, cursorPos, Quaternion.identity);   //투사체 프리팹을 커서 위치에 생성
        SubProjectile sub = proj.GetComponent<SubProjectile>();    //생성된 투사체 프리팹 가져오기
        if (sub != null)
        {
            sub.Init(equippedSubWeapon, target);   //타켓 좌표와 장착된 보조무기 정보를 넘기고 초기화
        }
        else
        {
            Debug.LogError("SubProjectile 컴포넌트가 프리팹에 없습니다!");
        }
    }


    void ShootAreaAroundCursor()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0f;

        int damage = Mathf.RoundToInt(equippedSubWeapon.GetDamage());
        float radius = equippedSubWeapon.projectileMaxDistance;

        // Layer 설정: "Monster" 레이어에만 영향
        LayerMask monsterLayer = LayerMask.GetMask("Monster");

        // ForceEffect 인스턴스 생성 (이름 없이 순서대로)
        IEffect forceEffect = new ForceEffect(cursorPos, damage, radius, monsterLayer);

        // EffectManager를 담은 임시 오브젝트 생성
        GameObject effectObj = new GameObject("ForceEffectHost");
        effectObj.transform.position = cursorPos;

        EffectManager effectManager = effectObj.AddComponent<EffectManager>();
        effectManager.Init(null); // 포스 이펙트는 특정 몬스터와 연결 없음
        effectManager.AddEffect(forceEffect);

        // 이펙트 종료 후 오브젝트 삭제
        Destroy(effectObj, 0.5f);
    }



    BaseMonster FindNearestAliveMonster(Vector3 from)     //가장 가까운 몬스터 탐색
    {
        BaseMonster[] monsters = FindObjectsOfType<BaseMonster>();    
        BaseMonster nearest = null;      
        float minDist = Mathf.Infinity;         //가장 가까운 거리(처음엔 무한대)

        foreach (var m in monsters)
        {
            if (m.IsDead) continue;   //이미 죽은 상태면 패스

            float dist = Vector2.Distance(from, m.transform.position);  //커서의 위치와 몬스터의 현재 거리를 계산
            if (dist < minDist)
            {
                minDist = dist;  //최소 거리 갱신
                nearest = m;     //가장 가까운 몬스터 저장
            }
        }

        return nearest;
    }

   

}
