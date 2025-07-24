using UnityEngine;

public class RadiantPulseSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;
    [SerializeField] private GameObject pulseEffectPrefab;

    private float cooldown = 10f;
    private float lastPulseTime = -10f;

    public void Init(SkillManager.SkillInstance instance)
    {
        skillInstance = instance;
        lastPulseTime = Time.time - cooldown; // 웨이브 시작 시 즉시 발동되도록
        Debug.Log("[빛의 파동] Init 호출됨");
    }

    void Update()
    {
        if (Time.time - lastPulseTime >= cooldown)
        {
            Debug.Log("[빛의 파동] 쿨타임 충족 - Pulse 발동 시도");
            TriggerPulse();
        }
    }

    private void TriggerPulse()
    {
        lastPulseTime = Time.time;
        float duration = skillInstance.GetCurrentLevelData().duration;

        GameObject pulseObj = Instantiate(pulseEffectPrefab, Vector3.zero, Quaternion.identity);
        Destroy(pulseObj, duration);
    }
}