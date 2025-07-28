using UnityEngine;

public class ThornDomeSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;
    [SerializeField] private GameObject thornDomePrefab;

    private float cooldown = 7f;
    private float timer = 0f;
    private GameObject activeThornDome;

    private Transform cursorTransform;
    private Camera mainCamera;

    public void Init(SkillManager.SkillInstance instance, Transform cursor)
    {
        Debug.Log("ThornDomeSkill Init 호출됨");
        skillInstance = instance;
        cursorTransform = cursor;
        mainCamera = Camera.main;
        timer = cooldown; // 웨이브 시작 시 즉시 발동 가능
    }
    public void UpdateLevel(SkillManager.SkillInstance instance)
    {
        Debug.Log("ThornDomeSkill UpdateLevel 호출됨");
        skillInstance = instance;
    }
    private void Update()
    {
        if (activeThornDome == null)
        {
            timer += Time.deltaTime;
        }
    }

    public void TryTriggerOnClick()
    {
        if (skillInstance == null || activeThornDome != null) return;

        if (timer < cooldown) return;
        if (!HasAtLeastOneMonster()) return;

<<<<<<< HEAD
        GameObject closest = FindClosestMonster();
        if (closest == null) return;

        // 방향 계산
        Vector2 dir = (closest.transform.position - cursorTransform.position).normalized;

        // 커서 기준 0.8f 떨어진 위치
        Vector3 spawnPos = cursorTransform.position + (Vector3)(dir * 0.3f);

        // 회전값 계산 (Z축 회전)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
<<<<<<< Updated upstream
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle); // 기본 이미지가 위를 보고 있으므로 -90도 보정
=======
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle); 
>>>>>>> Stashed changes

=======
        Vector3 spawnPosition = GetSpawnPositionNearClosestMonster();
>>>>>>> parent of 2752a4f (Test: 스킬 위치값 조정중)
        int damage = skillInstance.GetCurrentLevelData().damage;

        activeThornDome = Instantiate(thornDomePrefab, spawnPosition, Quaternion.identity);
        activeThornDome.GetComponent<ThornDome>().Init(damage, cursorTransform);

        timer = 0f;
    }

    private Vector3 GetSpawnPositionNearClosestMonster()
    {
        GameObject closest = FindClosestMonster();
        if (closest == null) return cursorTransform.position;

        Vector2 dir = (closest.transform.position - cursorTransform.position).normalized;
        float offsetDistance = 0.3f;
        return cursorTransform.position + (Vector3)(dir * offsetDistance);
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

    private bool HasAtLeastOneMonster()
    {
        return GameObject.FindGameObjectsWithTag("Monster").Length > 0;
    }
}