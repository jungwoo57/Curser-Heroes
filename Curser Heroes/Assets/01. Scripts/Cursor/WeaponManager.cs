using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }
    
    [Header("선택된 무기")]
    public WeaponData selectedWeapon;   // 인스펙터나 캐릭터 선택에서 지정

    [Header("무기 시스템")]
    public CursorWeapon cursorWeapon;
    public WeaponLife weaponLife;
    public WeaponUpgrade weaponUpgrade;
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
        selectedWeapon = GameManager.Instance.mainEquipWeapon.data;
        EquipWeapon(selectedWeapon);  // 게임 시작 시 한번만 호출됨
    }

    public void EquipWeapon(WeaponData weaponData)        //무기데이터를 받아 장착하는 함수
    {
        cursorWeapon.SetWeapon(weaponData);   
        weaponLife.Init(weaponData);        //무기 목숨 초기화
        weaponUpgrade.Init(weaponData);     //무기 레벨 초기화
        
        Debug.Log($"무기 장착 완료: {weaponData.weaponName}");
    }

    public void UpgradeWeapon()      
    {
        weaponUpgrade.Upgrade();      //외부에서 함수를 호출 하면 강화를 1회 한다.
    }

    public void TakeWeaponLifeDamage()  //데미지를 입었을 때 호출됨
    {
        AudioManager.Instance.PlayHitSound(HitType.Monster);
        weaponLife.TakeLifeDamage();            //목숨을 1개 줄이고 0이 되면 끝남
    }
}
