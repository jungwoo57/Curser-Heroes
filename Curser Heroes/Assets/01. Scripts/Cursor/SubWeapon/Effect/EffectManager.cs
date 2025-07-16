using UnityEngine;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour
{
    private BaseMonster owner;                   //이펙트 적용대상
    private readonly List<IEffect> activeEffects = new();  //이펙트 리스트

   
    public void Init(BaseMonster target)
    {
        owner = target;
        transform.parent = target.transform;      //매니저 오브젝트 하위에 붙이기
        transform.localPosition = Vector3.zero;   //위치를 몬스터 중심으로 초기화
    }

    public void AddEffect(IEffect effect)  //이펙트 추가
    {
        if (owner == null) return;

        effect.Apply(owner);
        activeEffects.Add(effect);
    }

    void Update()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)   //매 프레임마다 이펙트 업데이트
        {
            var effect = activeEffects[i];
            effect.Update(Time.deltaTime);

            if (effect.IsFinished)
                activeEffects.RemoveAt(i);  //끝난 이펙트는 제거
        }
    }
}
