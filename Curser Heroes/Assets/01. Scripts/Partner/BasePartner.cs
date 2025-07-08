// BasePartner.cs
using UnityEngine;

public abstract class BasePartner : MonoBehaviour
{
    // 에디터에서 할당할 ScriptableObject 형태의 동료 데이터
    public PartnerData data;

    // 몬스터가 데미지를 입을 때마다 게이지에 더해질 고정 값
    public float gaugePerMonsterHit = 2f;

    // 현재 게이지 값 (0 ~ data.gaugeMax)
    public float currentGauge;

    // 게이지 UI 컴포넌트
    private PartnerUI ui;

    // Awake 단계에서 UI를 찾아 초기 설정
    protected virtual void Awake()
    {
        // 자식 오브젝트에서 PartnerUI 컴포넌트를 찾아 할당
        ui = GetComponentInChildren<PartnerUI>(true);
        if (ui == null)
            Debug.LogError("PartnerUI를 자식 오브젝트에 꼭 부착해주세요.");
        // 초상화 설정 및 게이지 초기화
        ui.Configure(data.portraitSprite);
        ui.UpdateGauge(0f);
    }

    // 활성화 시 몬스터 피해 이벤트 구독
    protected virtual void OnEnable()
    {
        BaseMonster.OnAnyMonsterDamaged += HandleMonsterDamaged;
        BossStats.OnAnyMonsterDamaged += HandleMonsterDamaged;
    }

    // 비활성화 시 이벤트 구독 해제
    protected virtual void OnDisable()
    {
        BaseMonster.OnAnyMonsterDamaged -= HandleMonsterDamaged;
        BossStats.OnAnyMonsterDamaged -= HandleMonsterDamaged;

    }

    // 몬스터가 피해를 입을 때마다 호출되는 핸들러
    private void HandleMonsterDamaged(BaseMonster monster)
    {
        // 고정량만큼 게이지를 채우고 최대치를 넘지 않도록 제한
        currentGauge = Mathf.Min(currentGauge + gaugePerMonsterHit, data.gaugeMax);
        ui.UpdateGauge(currentGauge / data.gaugeMax);

        // 게이지가 가득 차면 스킬 발동 후 게이지 리셋
        if (currentGauge >= data.gaugeMax)
        {
         
            currentGauge = 0f;
            ui.UpdateGauge(0f);
        }
    }
    private void HandleMonsterDamaged(BossStats boss)
    {
        // 보스가 피해를 입을 때도 동일하게 게이지를 채움
        currentGauge = Mathf.Min(currentGauge + gaugePerMonsterHit, data.gaugeMax);
        ui.UpdateGauge(currentGauge / data.gaugeMax);
        // 게이지가 가득 차면 스킬 발동 후 게이지 리셋
        if (currentGauge >= data.gaugeMax)
        {
           
            currentGauge = 0f;
            ui.UpdateGauge(0f);
        }
    }

    // 각 파트너마다 고유 스킬 로직을 구현할 추상 메서드
    protected abstract void ActivateSkill();
}
