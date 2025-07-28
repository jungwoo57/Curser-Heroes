using UnityEngine;

public class ThornDomeSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;
    [SerializeField] private GameObject thornDomePrefab;

    private const float cooldown = 7f;
    private float timer = 0f;
    private GameObject activeThornDome;

    private Transform cursorTransform;

    public void Init(SkillManager.SkillInstance instance, Transform cursor)
    {
        skillInstance = instance;
        cursorTransform = cursor;
        timer = cooldown; // 시작 시 바로 발동 가능
    }

    public void UpdateLevel(SkillManager.SkillInstance instance)
    {
        skillInstance = instance;
    }

    private void Update()
    {
        if (activeThornDome == null)
            timer += Time.deltaTime;
    }

    public void TryTriggerOnClick()
    {
        if (skillInstance == null || activeThornDome != null) return;
        if (timer < cooldown) return;
        if (!HasAtLeastOneMonster()) return;

        GameObject closest = FindClosestMonster();
        if (closest == null) return;

        // 방향 계산
        Vector2 dir = (closest.transform.position - cursorTransform.position).normalized;

        // 커서 기준 0.8f 떨어진 위치
        Vector3 spawnPos = cursorTransform.position + (Vector3)(dir * 0.3f);

        // 회전값 계산 (Z축 회전)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle); // 기본 이미지가 위를 보고 있으므로 -90도 보정

        int damage = skillInstance.GetCurrentLevelData().damage;

        // 회전 적용
        activeThornDome = Instantiate(thornDomePrefab, spawnPos, rotation);
        activeThornDome.GetComponent<ThornDome>().Init(damage, cursorTransform, dir);

        timer = 0f;
    }

    private bool HasAtLeastOneMonster()
    {
        return GameObject.FindGameObjectsWithTag("Monster").Length > 0;
    }

    private GameObject FindClosestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (var m in monsters)
        {
            float dist = Vector2.Distance(cursorTransform.position, m.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = m;
            }
        }

        return closest;
    }
}