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

        GameObject closest = FindClosestMonster();
        if (closest == null) return;

        Vector3 spawnPosition = GetSpawnPositionNearClosestMonster();
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