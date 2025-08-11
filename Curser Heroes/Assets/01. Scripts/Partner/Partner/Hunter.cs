using System.Collections;
using UnityEngine;

public class Hunter : BasePartner
{
    [Header("스킬 범위")]
    public float skillRange = 20f;
    [SerializeField] GameObject[] hunterAnim;
    [SerializeField] GameObject[] arrowAnim;
    [SerializeField] private int arrowMax;
    [SerializeField] private int arrowMin;
    [SerializeField] private float xArea;
    [SerializeField] private float yArea;
    [SerializeField] private float waitTime;
    protected override void ActivateSkill()
    {
        arrowMax = arrowAnim.Length;
        for (int i = 0; i < hunterAnim.Length; i++)
        {
            hunterAnim[i].SetActive(true);
        }

        StartCoroutine(ArrowAnimation());
        StartCoroutine(AttackDelay());

        //스킬 사용 후 게이지 초기화
        currentGauge = 0f;
        ui.UpdateGauge(0f);
    }

    IEnumerator ArrowAnimation()
    {
        int count = 0;
        int arrowCount = Random.Range(arrowMin,arrowMax);
        while (count < arrowCount)
        {
            float x = Random.Range(-xArea,xArea);
            float y = Random.Range(-yArea,yArea);
            arrowAnim[count].transform.position = new Vector3(x,y,0f);
            arrowAnim[count].SetActive(true);
            yield return new WaitForSeconds(0.2f);
            count++;
        }

        for (int i = 0; i < arrowMax; i++)
        {
            arrowAnim[i].SetActive(false);
        }

        for (int i = 0; i < hunterAnim.Length; i++)
        {
            hunterAnim[i].SetActive(false);
        }
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(waitTime);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position,skillRange,LayerMask.GetMask("Monster"));

        //몬스터마다 1씩 피해 주기
        foreach (var hit in hits)
        {
            BaseMonster monster = hit.GetComponent<BaseMonster>();
            if (monster != null)
            {
                monster.TakeDamage(1, null);
                Debug.Log($"Hunter 스킬 발동: {monster.name}에게 1의 피해를 입힘");
            }
        }

    }
}

