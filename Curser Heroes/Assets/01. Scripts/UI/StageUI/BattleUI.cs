using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BattleUI : MonoBehaviour
{
    public GameObject battlePanel;
    public SkillManager skillManager;

    [Header("플레이어 체력 UI")]
    [SerializeField]private GameObject[] healthImage;
    [SerializeField]private Sprite activeHealthImage;
    [SerializeField]private Sprite inactiveHealthImage;
    [SerializeField]private int healthIndex;


    [Header("동료 UI")] 
    [SerializeField] private GameObject partnerImage;
    [SerializeField] private Image partnerHealth;

    [Header("정보 UI")] 
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI jewelText;
    
    [Header("스킬UI")]
    [SerializeField] public SkillUI[] skills;
    [SerializeField] public int skillIndex;
    
    
    [Header("더미 데이터")]  // 테스트용 데이터 추후 삭제
    [Range(0,10)]public float partnerCurHealth;
    public float partnerMaxHealth;
    public int playerMaxHelath;


    private void Start()
    {
        Init();
    }


    private void Update()
    {
        PartnerHealthUpdate();
    }


    public void Init() // 스테이지 시작 시 초기화 함수
    {
        battlePanel.SetActive(true);
        //healthIndex = playerMaxHelath - 1;// 더미 데이터 추후 변경  동료 데이터 추가 시 동료 데이터도 초기화
       
        healthIndex = WeaponManager.Instance.weaponLife.currentWeapon.maxLives; //변경할 코드   

        if (healthIndex > playerMaxHelath)
        {
            healthIndex = playerMaxHelath-1;
        }
        for (int i = 0; i < healthIndex; i++) // 임시코드 추후 플레이어 maxHp로 로직 변경
        {
            healthImage[i].GetComponent<Image>().sprite = activeHealthImage;
            healthImage[i].SetActive(true);
        }
        TextUpdate();
    }
    
    [ContextMenu("데미지주기")]
    public void TakeDamage()//데미지를 입었을 때
    { 
        if (healthIndex > 0)
        {
            healthIndex--;
            healthImage[healthIndex].GetComponent<Image>().sprite = inactiveHealthImage;
        }
    }

    
    [ContextMenu("힐하기")]
    public void Heal()
    {
        if (healthIndex < playerMaxHelath-1)
        {
            healthIndex++;
        }
        healthImage[healthIndex].GetComponent<Image>().sprite = activeHealthImage;
    }

    public void PartnerHealthUpdate() // 동료 체력 업데이트 피격 시 실행
    {
        float percent = partnerCurHealth / partnerMaxHealth;
        partnerHealth.fillAmount = percent;
    }

    public void TextUpdate()
    {
        stageText.text = "Stage : " + 0; // 매니저에게 스테이지 정보 가져와서 적용
        waveText.text = "Wave : " + 0; //매니저에게 스테이지 정보 가져와서 적용
        goldText.text = "Gold : " + GameManager.Instance.GetGold() + "(" + 0 + ")";  //매니저에게 스테이지 정보 가져와서 적용
        jewelText.text = "Jewel : " + GameManager.Instance.GetJewel() + "(" + 0 + ")"; //매니저에게 스테이지 정보 가져와서 적용
    }

    public void SkillUpdate()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (i < skillManager.ownedSkills.Count)
            {
                skills[i].SetSkill(skillManager.ownedSkills[i]);
                skills[i].gameObject.SetActive(true);
            }
            else
            {
                skills[i].gameObject.SetActive(false);
            }
        }
    }
}
