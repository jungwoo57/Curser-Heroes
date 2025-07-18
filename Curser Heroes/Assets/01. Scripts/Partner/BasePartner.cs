
using UnityEngine;


public abstract class BasePartner : MonoBehaviour
{
    [Header("기본 데이터")]
    public PartnerData data;                     // ScriptableObject 데이터 (에디터에서 할당)
    public int level;
    
    [Header("게이지 설정")]
    public float gaugePerMonsterHit = 2f;        // 몬스터 한 마리 맞을 때마다 오를 게이지 값
    [HideInInspector] public float currentGauge; // 현재 게이지 값

    protected PartnerUI ui;                      // 게이지 및 초상화 UI

    
    protected virtual void Awake()
    {
        if (data == null)
        {
            Debug.LogError($"{name}: PartnerData가 할당되지 않았습니다!");
            return;
        }

        ui = GetComponentInChildren<PartnerUI>(true);
        if (ui == null)
        {
            Debug.LogError($"{name}: 자식 오브젝트에 PartnerUI가 없습니다!");
            return;
        }

        ui.Configure(data.portraitSprite);
        ResetGauge();
    }

   
    protected virtual void OnEnable()
    {
        CursorWeapon.OnAnyMonsterDamaged += HandleMonsterDamaged;
    }

    
    protected virtual void OnDisable()
    {
        CursorWeapon.OnAnyMonsterDamaged -= HandleMonsterDamaged;
    }

   
    private void HandleMonsterDamaged(CursorWeapon monster)
    {
        if (currentGauge >= data.gaugeMax)
            return; // 중복 발동 방지

        currentGauge = Mathf.Min(currentGauge + gaugePerMonsterHit, data.gaugeMax);
        ui.UpdateGauge(currentGauge / data.gaugeMax);

        if (currentGauge >= data.gaugeMax)
        {
            ActivateSkill();
            ResetGauge();
        }
    }

  
    protected void ResetGauge()
    {
        currentGauge = 0f;
        ui.UpdateGauge(0f);
    }

    protected abstract void ActivateSkill();
}
