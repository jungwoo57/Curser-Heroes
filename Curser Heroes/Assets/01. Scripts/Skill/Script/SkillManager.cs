using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GameObject rewardSelectUIPrefab;
    [SerializeField] private GameObject skillSelectUIPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private CursorWeapon cursorWeapon;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject explodeOnKillSkillPrefab;
    [SerializeField] private GameObject indomitableSkillPrefab;
    [SerializeField] private GameObject lightningEffectPrefab;

    public static SkillManager Instance { get; private set; }
    public List<SkillData> skillPool = new List<SkillData>();
    public List<SkillInstance> ownedSkills = new List<SkillInstance>();

    public CursorWeapon CursorWeapon => cursorWeapon;
    public int maxSkillCount = 6;

    private GameObject skillSelectUIInstance;
    private Dictionary<SkillData, GameObject> persistentSkillObjects = new Dictionary<SkillData, GameObject>();

    // ExplodeOnKillSkill 인스턴스 캐싱
    private ExplodeOnKillSkill explodeSkillComponent;
    private const string FIREBALL_SKILL_NAME = "화염구";
    private IndomitableSkill indomitableSkillInstance;
    private LightningSkill lightningSkill;

    private float bonusSubWeaponDamage = 0f;
    public float BonusSubWeaponDamage => bonusSubWeaponDamage;
    public int criticalSweepEveryNth = 0;

    [System.Serializable]
    public class SkillInstance
    {
        public SkillData skill;
        public int level;
        public bool IsMaxed => level >= skill.maxLevel;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        ExplodeOnKillSkill.explosionPrefab = explosionPrefab;

        // 자동 생성
        if (FindObjectOfType<ExplodeOnKillSkill>() == null && explodeOnKillSkillPrefab != null)
        {
            Instantiate(explodeOnKillSkillPrefab);
        }
        var lightningInstance = ownedSkills.FirstOrDefault(s => s.skill.skillName == "라이트닝");
        if (lightningInstance != null && lightningSkill == null)
        {
            GameObject obj = new GameObject("LightningSkill");
            lightningSkill = obj.AddComponent<LightningSkill>();
            lightningSkill.Init(lightningInstance);

            // 번개 이펙트 프리팹과 몬스터 레이어 설정은 직접 연결 필요
            lightningSkill.lightningEffectPrefab = lightningEffectPrefab;
            lightningSkill.monsterLayerMask = LayerMask.GetMask("Monster");
        }

    }

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance가 null입니다.");
            return;
        }
        // skillPool 동기화
        skillPool = new List<SkillData>(GameManager.Instance.skillPool);

        Debug.Log($"[SkillManager] skillPool 크기: {skillPool.Count}");

        if (player == null)
            Debug.LogError("SkillManager의 player Transform이 null입니다!");
    }

    public void OnWaveEnd()
    {
        if (ownedSkills.Count == maxSkillCount && ownedSkills.All(s => s.IsMaxed))
        {
            ShowRewardSelection();
        }
        else
        {
            ShowSkillSelection();
        }
    }

    public bool HasSkill(string skillName)
    {
        return ownedSkills.Any(s => s.skill.skillName == skillName);
    }

    void ShowSkillSelection()
    {
        if (skillSelectUIPrefab == null)
        {
            Debug.LogError("SkillSelectUIPrefab이 연결되어 있지 않습니다.");
            return;
        }

        skillSelectUIInstance = Instantiate(skillSelectUIPrefab);
        var canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            skillSelectUIInstance.transform.SetParent(canvas.transform, false);
        }

        var skillUI = skillSelectUIInstance.GetComponent<SkillSelectUI>();
        if (skillUI == null)
        {
            Debug.LogError("SkillSelectUI 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        List<SkillData> availableSkills = skillPool
            .Where(skill =>
                !ownedSkills.Any(own => own.skill == skill && own.IsMaxed) &&
                (ownedSkills.Count < maxSkillCount || ownedSkills.Any(own => own.skill == skill))
            ).ToList();

        List<SkillData> selection = GetRandomSkills(availableSkills, 3);
        skillUI.Show(selection, OnSkillSelected);
    }

    void OnSkillSelected(SkillData selected)
    {
        var owned = ownedSkills.FirstOrDefault(s => s.skill == selected);
        if (owned != null)
        {
            owned.level++;
        }
        else
        {
            owned = new SkillInstance { skill = selected, level = 1 };
            ownedSkills.Add(owned);
        }
        if (selected.skillName == "근력 강화")
        {
            var strengthSkill = FindObjectOfType<StrengthTrainingSkill>();
            if (strengthSkill == null)
            {
                GameObject obj = new GameObject("StrengthTrainingSkill");
                strengthSkill = obj.AddComponent<StrengthTrainingSkill>();
            }
            strengthSkill.Init(owned);
        }
        // 레벨업 또는 신규 습득 후 자동 배치
        if (selected.skillName == "매직소드" || selected.skillName == "포이즌필드" || selected.skillName == "수호의 방패" 
            || selected.skillName == "불굴" || selected.skillName == "구원" || selected.skillName == "아이스 에이지")
        {
            DeployPersistentSkill(owned);
        }

        Debug.Log($"[SkillManager] 스킬 선택됨: {selected.skillName}");

        if (UIManager.Instance == null)
        {
            Debug.LogError("UIManager.Instance is null!");
        }
        else if (UIManager.Instance.battleUI == null)
        {
            Debug.LogError("UIManager.Instance.battleUI is null!");
        }
        else
        {
            Debug.Log("[SkillManager] SkillUpdate 호출");
            UIManager.Instance.battleUI.SkillUpdate();
        }

        Destroy(skillSelectUIInstance);
        skillSelectUIInstance = null;

        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.IncrementWaveIndex();
            WaveManager.Instance.StartWave();

            ApplySkillEffects();
        }
        else
        {
            Debug.LogError("WaveManager.Instance가 null입니다.");
        }
    }

    public void DeployPersistentSkill(SkillInstance skillInstance)
    {
        if (cursorWeapon == null)
        {
            Debug.LogWarning("CursorWeapon 참조가 없습니다.");
            return;
        }

        SkillData skillData = skillInstance.skill;

        if (skillData.skillName == "불굴")
        {
            if (indomitableSkillInstance == null)
            {
                GameObject obj = Instantiate(indomitableSkillPrefab, WeaponManager.Instance.cursorWeapon.transform.position, Quaternion.identity);
                indomitableSkillInstance = obj.GetComponent<IndomitableSkill>();
                indomitableSkillInstance.Init(skillInstance, WeaponManager.Instance.cursorWeapon.transform);
                WeaponManager.Instance.indomitableSkillInstance = indomitableSkillInstance;
            }
            else
            {
                Debug.Log("[SkillManager] 불굴 스킬 이미 존재");
            }
            return;
        }
        if (skillData.skillName == "구원")
        {
            GameObject go = Instantiate(skillData.skillPrefab);
            SalvationSkill salvation = go.GetComponent<SalvationSkill>();
            salvation.Init(skillInstance, WeaponManager.Instance.cursorWeapon.transform);

            WeaponManager.Instance.salvationSkillInstance = salvation;

            return;
        }

        if (persistentSkillObjects.TryGetValue(skillData, out GameObject existingObj))
        {
            Debug.Log($"[SkillManager] 지속형 스킬 이미 설치됨: {skillData.skillName}");

            if (existingObj.TryGetComponent(out RotatingSkill rotating))
            {
                rotating.UpdateSwords(skillInstance);
            }
            else if (existingObj.TryGetComponent(out AoEFieldSkill aoe))
            {
                aoe.Init(skillInstance, cursorWeapon.transform);
            }
            else if (existingObj.TryGetComponent(out RotatingShieldSkill shield))
            {
                shield.UpdateShields(skillInstance);
            }
            return;
        }
        else if (skillData.skillName == "아이스 에이지")
        {
            GameObject obj = Instantiate(skillData.skillPrefab, cursorWeapon.transform.position, Quaternion.identity);
            persistentSkillObjects[skillData] = obj;

            IceAgeSkill iceSkill = obj.GetComponent<IceAgeSkill>();
            if (iceSkill != null)
            {
                
                iceSkill.Init(skillInstance);
            }
            else
            {
                Debug.LogWarning("IceAgeSkill 컴포넌트를 찾을 수 없습니다.");
            }
            return;
        }

        Vector3 spawnPos = cursorWeapon.transform.position;
        GameObject newObj = Instantiate(skillData.skillPrefab, spawnPos, Quaternion.identity);
        persistentSkillObjects[skillData] = newObj;

        if (newObj.TryGetComponent(out RotatingShieldSkill shieldSkill))
        {
            shieldSkill.Init(skillInstance, cursorWeapon.transform);
        }
        else if (newObj.TryGetComponent(out RotatingSkill rotatingSkill))
        {
            rotatingSkill.Init(skillInstance, cursorWeapon.transform);
        }
        else if (newObj.TryGetComponent(out AoEFieldSkill aoeSkill))
        {
            aoeSkill.Init(skillInstance, cursorWeapon.transform);
        }
        else
        {
            Debug.LogWarning($"[SkillManager] {skillData.skillName}에 맞는 SkillBehaviour가 없습니다.");
        }
    }
    public void ApplySkillEffects()
    {
        bonusSubWeaponDamage = 0f;

        WeaponManager.Instance.invincibilityTime = 3.0f;

        foreach (var instance in ownedSkills)
        {
            var skill = instance.skill;
            int level = Mathf.Clamp(instance.level, 1, skill.levelDataList.Count);

            if (instance.skill.skillName == "집중 훈련")
            {
                bonusSubWeaponDamage += instance.skill.levelDataList[level - 1].damage; 
                Debug.Log($"[SkillManager] 집중 훈련 Lv.{level} → 보조무기 데미지 보너스: +{bonusSubWeaponDamage}");
            }

            if (skill.skillName == "느긋한 행동")
            {
                float extraTime = skill.levelDataList[level - 1].duration;
                WeaponManager.Instance.invincibilityTime += extraTime;
                Debug.Log($"[SkillManager] 느긋한 행동 Lv.{level} → 무적 시간 증가: +{extraTime}초 → 총 {WeaponManager.Instance.invincibilityTime}초");
            }
            if (skill.skillName == "약점 포착")
            {
                int triggerCount = skill.levelDataList[level - 1].count;
                SkillManager.Instance.criticalSweepEveryNth = triggerCount;

                Debug.Log($"[SkillManager] 약점 포착 Lv.{level} → {triggerCount}번째 스윕마다 2배 피해");
            }
        }
    }

    public void TryShootFireball()
    {
        SkillInstance fireballSkill = ownedSkills.FirstOrDefault(s => s.skill.skillName == FIREBALL_SKILL_NAME);
        if (fireballSkill == null)
            return;

        float procChance = 0.4f;
        if (Random.value > procChance)
            return;

        BaseMonster[] allMonsters = FindObjectsOfType<BaseMonster>();
        BaseMonster nearest = null;
        float minDist = Mathf.Infinity;

        // 마우스 커서 월드 좌표 얻기
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0f;

        foreach (var monster in allMonsters)
        {
            if (monster.IsDead) continue;

            float dist = Vector2.Distance(cursorPos, monster.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = monster;
            }
        }

        if (nearest == null) return;

        SkillLevelData levelData = fireballSkill.skill.levelDataList[fireballSkill.level - 1];
        int damage = levelData.damage;

        GameObject prefab = fireballSkill.skill.skillPrefab;

        // 마우스 커서 위치에서 발사
        GameObject fireball = Instantiate(prefab, cursorPos, Quaternion.identity);

        if (fireball.TryGetComponent(out FireballSkill fireballComp))
        {
            // 방향 계산: 타겟 위치 - 커서 위치
            Vector3 direction = (nearest.transform.position - cursorPos).normalized;
            fireballComp.Init(damage, direction);
        }
        else
        {
            Debug.LogWarning("FireballSkill 컴포넌트를 찾을 수 없습니다.");
        }
    }

    public void OnMonsterKilled(Vector3 deathPosition)
    {
        SkillInstance explodeSkill = ownedSkills.Find(s => s.skill.skillName == "장렬한 퇴장");
        if (explodeSkill == null) return;

        SkillLevelData data = explodeSkill.skill.levelDataList[explodeSkill.level - 1];
        int damage = data.damage;
        float radius = 1.5f;

        if (explodeSkillComponent != null)
        {
            explodeSkillComponent.TriggerExplosion(deathPosition, damage, radius, LayerMask.GetMask("Monster"));
        }
        else
        {
            Debug.LogWarning("ExplodeOnKillSkill 컴포넌트가 없습니다!");
        }
    }

    void ShowRewardSelection()
    {
        GameObject canvas = GameObject.Find("Canvas"); 
        if (canvas == null)
        {
            Debug.LogError("Canvas를 찾을 수 없습니다.");
            return;
        }

        GameObject ui = Instantiate(rewardSelectUIPrefab, canvas.transform, false);
        ui.GetComponent<RewardSelectUI>().Init(OnRewardSelected);
    }

    void OnRewardSelected(int index)
    {
        switch (index)
        {
            case 0:WeaponManager.Instance.weaponLife.RecoverLife(); break;
            case 1: GameManager.Instance.AddGold(100); break;
            case 2: GameManager.Instance.AddJewel(10); break;
        }
        WaveManager.Instance.IncrementWaveIndex();
        WaveManager.Instance.StartWave();
    }

    List<SkillData> GetRandomSkills(List<SkillData> source, int count)
    {
        return source.OrderBy(x => Random.value).Take(count).ToList();
    }

    public void TrySpawnMeteorSkill(SkillInstance skillInstance)
    {
        float procChance = 0.25f;

        if (Random.value > procChance)
            return;

        List<Transform> monsters = new List<Transform>();

        foreach (var monster in FindObjectsOfType<BaseMonster>())
            monsters.Add(monster.transform);

        if (monsters.Count == 0)
            return;

        Transform target = monsters[Random.Range(0, monsters.Count)];

        GameObject meteorPrefab = skillInstance.skill.skillPrefab;
        GameObject meteorObj = Instantiate(meteorPrefab);

        var meteor = meteorObj.GetComponent<MeteorSkill>();
        var levelData = skillInstance.skill.levelDataList[skillInstance.level - 1];
        meteor.Init(levelData.damage, target.position);
    }
}