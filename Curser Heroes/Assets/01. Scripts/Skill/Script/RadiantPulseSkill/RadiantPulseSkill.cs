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
    }

    void Update()
    {
        if (Time.time - lastPulseTime >= cooldown)
        {
            TriggerPulse();
        }
    }

    private void TriggerPulse()
    {
        lastPulseTime = Time.time;

        float duration = skillInstance.GetCurrentLevelData().duration;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // 2D 기준

        GameObject pulseObj = Instantiate(pulseEffectPrefab, mouseWorldPos, Quaternion.identity);

        // 마우스를 따라가므로 Init() 필요 없음
        Destroy(pulseObj, duration);
    }
}