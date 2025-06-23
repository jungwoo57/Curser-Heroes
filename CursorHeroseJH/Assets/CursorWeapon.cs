using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CursorWeapon : MonoBehaviour
{
    public float attackRange = 0.5f;       //공격범위 0.5(기본설정)
    public int attackPower = 10;           //기본 데미지 10
    public float attackCooldown = 0.2f;    // 어택 쿨타임 0.2초
    public LayerMask targetLayer;  //타겟을 레이어로 지정

    
    private Dictionary<Monster, float> lastHitTimes = new Dictionary<Monster, float>();

    private Camera cam;

    void Start()
    {
        cam = Camera.main;      //메인 카메라 찾기

    }

    void Update()
    {
        AutoAttackCursor();      //마우스 커서가 몬스터에 닿았을 때 자동으로 데미지가 들어가는 함수
    }

    private void AutoAttackCursor()
    {
        Vector3 mousePos = Input.mousePosition;       //현재 화면의 마우스 커서 위치
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos); // 마우스 커서의 위치를 월드 좌표로 바꿈
        Vector2 cursorPos = new Vector2(worldPos.x, worldPos.y);  //2d게임이기에 z값을 제외한 2d좌표로 설정

        Collider2D[] hits = Physics2D.OverlapCircleAll(cursorPos, attackRange, targetLayer);
        //마우스 커서의 크기만큼 주변의 원이 있다는 가정하에 안에 있는 콜라이더들을 찾아서 hits 배열에 넣는다

        foreach (Collider2D hit in hits)                              
        
        {
            Monster monster = hit.GetComponent<Monster>();
            if (monster == null) continue;

            float lastHitTime;   
            bool found = lastHitTimes.TryGetValue(monster, out lastHitTime); 
            //몬스터가 딕셔너리에 있는지 파악하고 피격 여부를 통해 마지막으로 데미지가 들어간 시간을 정함

            if (Time.time - lastHitTime >= attackCooldown)      //쿨타임이 지났다면 다시 공격 가능
            {
                monster.TakeDamage(attackPower);
                lastHitTimes[monster] = Time.time;
            }
        }
    }
}
    

