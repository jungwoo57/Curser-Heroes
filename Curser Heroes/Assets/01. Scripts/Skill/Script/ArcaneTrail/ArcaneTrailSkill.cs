using UnityEngine;

public class ArcaneTrailSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;

    private float cooldown;
    private float timer;

    public void Init(SkillManager.SkillInstance instance)
    {
        skillInstance = instance;

        // 쿨다운은 6 - (레벨 - 1) * 1.0f 초
        int level = instance.level;
        cooldown = 5f;
        timer = 0f;
    }

    void Update()
    {
        if (!WaveManager.Instance.IsWavePlaying) return;

        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            // 실제 마우스 위치를 월드 좌표로 변환
            Vector3 mouseScreenPos = Input.mousePosition;
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            spawnPos.z = 0f; // 2D 게임이므로 z값 고정

            int damage = skillInstance.GetCurrentLevelData().damage;
            Instantiate(skillInstance.skill.skillPrefab, spawnPos, Quaternion.identity)
                .GetComponent<ArcaneTrail>().Init(damage);

            timer = 0f;
        }
    }
}