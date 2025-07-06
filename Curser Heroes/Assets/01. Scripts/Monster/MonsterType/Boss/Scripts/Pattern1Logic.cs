using UnityEngine;
using System.Collections;

public class Pattern1Logic : PatternLogicBase
{
    [Header("Pattern1 부모 오브젝트 (Pattern 1)")]
    public Transform patternParent;

    [Header("패턴 시작 전 지연 (초)")]
    public float startDelay = 1.0f;          // 패턴이 시작되기 전 기다릴 시간

    [Header("각 자식 활성화 간격 (초)")]
    public float activationInterval = 0.1f;  // 자식 간 켜지는 간격

    public override IEnumerator Execute(BossPatternController controller)
    {
        if (patternParent == null)
        {
            Debug.LogWarning("Pattern1Logic: patternParent가 할당되지 않았습니다.");
            yield break;
        }

        // 1) 패턴 시작 전 지연
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        int childCount = patternParent.childCount;
        if (childCount == 0)
        {
            Debug.LogWarning("Pattern1Logic: 자식 오브젝트가 없습니다.");
            yield break;
        }

        // 2) 모든 자식 비활성화 (초기화)
        for (int i = 0; i < childCount; i++)
            patternParent.GetChild(i).gameObject.SetActive(false);

        // 3) 자식 하나씩 순차 활성화 (켜진 것은 유지)
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = patternParent.GetChild(i).gameObject;

            // (선택) 개별 추가 지연이 필요하면 여기도 yield 추가
            // yield return new WaitForSeconds(0.5f);

            child.SetActive(true);                     // 켜기
            yield return new WaitForSeconds(activationInterval);
        }

        // 4) 애니메이션 종료 후(또는 여기에 추가 대기) 한 번에 모두 끄고 마무리
        for (int i = 0; i < childCount; i++)
            patternParent.GetChild(i).gameObject.SetActive(false);
    }
}
