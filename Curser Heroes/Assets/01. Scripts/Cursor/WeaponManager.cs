using System.Collections;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [Header("선택된 무기")] public WeaponData selectedWeapon; // 인스펙터나 캐릭터 선택에서 지정

    [Header("무기 시스템")] public CursorWeapon cursorWeapon;
    public WeaponLife weaponLife;
    public WeaponUpgrade weaponUpgrade;

    [Header("스킬 시스템")]
    public IndomitableSkill indomitableSkillInstance;
    public SalvationSkill salvationSkillInstance;

    [Header("기타 정보")]
    public bool isDie= false;
    public bool isInvincible = false;
    public float invincibilityTime = 3.0f;   //무적 시간
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 제거
            return;
        }

        Instance = this;
    }

    void Start()
    {
        Cursor.visible = false; 
        selectedWeapon = GameManager.Instance.mainEquipWeapon.data;
        EquipWeapon(selectedWeapon); // 게임 시작 시 한번만 호출됨
    }

    public void EquipWeapon(WeaponData weaponData) //무기데이터를 받아 장착하는 함수
    {
        cursorWeapon.SetWeapon(weaponData);
        weaponLife.Init(weaponData); //무기 목숨 초기화
        weaponUpgrade.Init(weaponData); //무기 레벨 초기화

        Debug.Log($"무기 장착 완료: {weaponData.weaponName}");
    }

    public void UpgradeWeapon()
    {
        weaponUpgrade.Upgrade(); //외부에서 함수를 호출 하면 강화를 1회 한다.
    }

    public void TakeWeaponLifeDamage()
    {
        if (isDie) return;
        if (isInvincible) return;

        // 불굴은 선제적으로 막음 (맞기 전에)
        if (indomitableSkillInstance != null && indomitableSkillInstance.TryBlockDamage())
        {
            Debug.Log("[WeaponManager] 불굴로 피해 무효화!");
            return;
        }

        AudioManager.Instance.PlayHitSound(HitType.Monster);

        // 🔥 실제로 체력 깎기
        weaponLife.TakeLifeDamage();

        // ⚠️ 체력 0이 된 뒤 구원 시도
        if (weaponLife.currentLives <= 0)
        {
            if (salvationSkillInstance != null && salvationSkillInstance.TryActivate())
            {
                Debug.Log("[WeaponManager] 구원 스킬로 사망 방지!");
                return;
            }

            // 진짜 죽는 경우
            isDie = true;
            UIManager.Instance.isStart = false;
            UIManager.Instance.stageExitPanel.gameObject.SetActive(false);
            StartCoroutine(DieAnimation());
            return;
        }

        // 죽지 않았으면 무적 처리
        if (!isDie)
        {
            StartCoroutine(OnInvincible());
        }
    }

    public void TakeWeaponLifeBossDamage()
    {
        if (isDie) return;
        if (isInvincible) return;

        // 불굴은 선제적으로 막음 (맞기 전에)
        if (indomitableSkillInstance != null && indomitableSkillInstance.TryBlockDamage())
        {
            Debug.Log("[WeaponManager] 불굴로 피해 무효화!");
            return;
        }

        AudioManager.Instance.PlayHitSound(HitType.Monster);

        // 🔥 실제로 체력 깎기
        weaponLife.TakeLifeBossDamage();

        // ⚠️ 체력 0이 된 뒤 구원 시도
        if (weaponLife.currentLives <= 0)
        {
            if (salvationSkillInstance != null && salvationSkillInstance.TryActivate())
            {
                Debug.Log("[WeaponManager] 구원 스킬로 사망 방지!");
                return;
            }

            // 진짜 죽는 경우
            isDie = true;
            UIManager.Instance.isStart = false;
            UIManager.Instance.stageExitPanel.gameObject.SetActive(false);
            StartCoroutine(DieAnimation());
            return;
        }

        // 죽지 않았으면 무적 처리
        if (!isDie)
        {
            StartCoroutine(OnInvincible());
        }
    }
    
    private IEnumerator OnInvincible()
    {
        isInvincible = true;
        SpriteRenderer cursorImage = cursorWeapon.GetComponent<SpriteRenderer>();
        Material cursorMaterial = cursorImage.material;
        float elapsedTime = 0f;
        float duration = 0.2f;
        bool isBlink = false;
        float blinkTime = 0.5f;
        
        while(elapsedTime < invincibilityTime)
        {
            if (cursorImage)
            {
                isBlink = !isBlink;
                if (duration - blinkTime <= 0)
                {
                    cursorMaterial.SetFloat("_FlashAmount", isBlink ? 1.0f : 0f);
                    blinkTime = 0;
                }
                yield return null;
                blinkTime += Time.deltaTime;
                elapsedTime += Time.deltaTime;
            }
        }
        cursorMaterial.SetFloat("_FlashAmount", 0f);
        isInvincible = false;
    }
    // 아래 코드는 구원 스킬 사용 시 무적판정을 위해 사용됨
    public IEnumerator OnTemporaryInvincible(float duration)
    {
        isInvincible = true;

        SpriteRenderer cursorImage = cursorWeapon.GetComponent<SpriteRenderer>();
        Material cursorMaterial = cursorImage.material;
        float elapsedTime = 0f;
        bool isBlink = false;

        while (elapsedTime < duration)
        {
            isBlink = !isBlink;
            if (cursorImage)
                cursorMaterial.SetFloat("_FlashAmount", isBlink ? 1f : 0f);

            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
        }

        if (cursorImage)
            cursorMaterial.SetFloat("_FlashAmount", 0f);

        isInvincible = false;
    }

    private IEnumerator DieAnimation()         //커서 죽었을때 호출
    {
        float rotationSpeed = 720.0f; // 초당 회전수
        float moveSpeed = 1.5f; // 올라가는 속도
        Vector3 fallPosition = new Vector3(0, -7.0f, 0);   //떨어질 위치
        float elaspedTime = 0;
        while (cursorWeapon.transform.position.y > fallPosition.y)
        {
            if (elaspedTime < 0.5) cursorWeapon.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            else cursorWeapon.transform.position += Vector3.down * moveSpeed * Time.deltaTime * (elaspedTime*2.0f);
            float angle = rotationSpeed * Time.deltaTime;
            cursorWeapon.transform.rotation *= Quaternion.Euler(0, 0, -angle);
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        UIManager.Instance.StageEnd();
        Cursor.visible = true;
    }
}
        

