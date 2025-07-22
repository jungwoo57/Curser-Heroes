using UnityEngine;

public class ArcaneTrailSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;

    [SerializeField] private GameObject arcaneTrailPrefab;
    private float cooldown = 5f;
    private float timer = 0f;

    private Camera mainCamera;

    public void Init(SkillManager.SkillInstance instance)
    {
        skillInstance = instance;
        timer = cooldown; // 웨이브 시작 시 즉시 발동
        mainCamera = Camera.main;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            timer = 0f;

            // 커서 위치 가져오기
            Vector3 spawnPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            spawnPos.z = 0f;
            spawnPos += Vector3.down * 0.8f;

            int damage = skillInstance.GetCurrentLevelData().damage;

            GameObject obj = Instantiate(arcaneTrailPrefab, spawnPos, Quaternion.identity);
            obj.GetComponent<ArcaneTrail>().Init(damage);
        }
    }
}