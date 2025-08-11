using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ForgeUI : MonoBehaviour
{
    public Image weaponImage;
    public Image WeaponTextInfo;
    
    [Header("텍스트 목록")]
    public TextMeshProUGUI hasGoldText;
    public TextMeshProUGUI useGoldText;
    public TextMeshProUGUI hasJewelText;
    public TextMeshProUGUI useJewelText;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponDesc;
    public TextMeshProUGUI weaponHp;
    public TextMeshProUGUI weaponAtk;
    public TextMeshProUGUI upgradeWeaponAtk;
    public TextMeshProUGUI currentWeaponAtk;
    [SerializeField] private TextMeshProUGUI reinforceText;
    [SerializeField] private TextMeshProUGUI unlockText;

    [Header("이미지 목록")] 
    [SerializeField] private Image goldImage;
    [SerializeField] private Image jewelImage;
    
    
    [Header("무기 선택 이미지")] 
    public ForgeWeaponUI[] weaponUIs;
    
    [Header("버튼 목록")]
    public Button reinforceButton;
    public Button mainWeaponButton;
    public Button subWeaponButton;
    public Button unlockButton;

    [Header("텍스트 컬러")] 
    [SerializeField] private Color enabledColor;
    [SerializeField] private Color disableColor;


    [Header("강화 표시 및 해금 이펙트")] 
    [SerializeField] private GameObject[] upgradeDirection;
    [SerializeField] private GameObject unlockDirection;
    [SerializeField] private float effectDurationTime;
    
    public bool isMain = true;
    public OwnedWeapon selectWeapon;
    public OwnedSubWeapon selectSubWeapon;
	public WeaponData selectData;
	public SubWeaponData selectSubData;
	
    
    public TutorialImageUI tutorialImageUI;
    [SerializeField] private ScrollRect weaponScroll;
    private void OnEnable()
    {
        isMain = true;
        mainWeaponButton.interactable = false;
        subWeaponButton.interactable = true;
        weaponScroll.verticalNormalizedPosition = 1.0f;
        Init();
        if (!GameManager.Instance.useForge)
        {
            ClickHintButton();
            GameManager.Instance.useForge = true;
            GameManager.Instance.Save();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !tutorialImageUI.gameObject.activeInHierarchy)
        {
            ClickExitButton();
        }
    }
    public void Init()
    {
        if (GameManager.Instance.mainEquipWeapon.data == null)     //장착 무기 없으면 1번 착용
        {
            selectWeapon = GameManager.Instance.ownedWeapons[0];
            selectData = GameManager.Instance.ownedWeapons[0].data;
        }
        else
        {
            selectWeapon = GameManager.Instance.mainEquipWeapon;
            selectData = GameManager.Instance.mainEquipWeapon.data;
        }

        if (GameManager.Instance.subEquipWeapon.data == null)
        {
            selectSubWeapon = GameManager.Instance.ownedSubWeapons[0];
            selectSubData = GameManager.Instance.ownedSubWeapons[0].data;
        }
        else
        {
            selectSubWeapon = GameManager.Instance.subEquipWeapon;
            selectSubData = GameManager.Instance.subEquipWeapon.data;
        }
        UIUpdate();
        UpdateSelectUI();
    }
    
    public void OnClickReinforceButton()
    {
        if (isMain)
        {
            if (GameManager.Instance.GetGold() >= selectWeapon.data.upgradeCost[selectWeapon.level])
            {
                GameManager.Instance.UpgradeWeapon(selectWeapon.data);
                UIUpdate();
            }
        }
        else
        {
            if (GameManager.Instance.GetGold() >= selectSubWeapon.data.upgradeCost[selectSubWeapon.level])
            {
                GameManager.Instance.UpgradeWeapon(selectSubWeapon.data);
                UIUpdate();
            }
        } //weapondata에 레벨이 존재해야 할 것 같음
        ReinforceEffect();
    }

    public void OnClickUnlockWeapon()
    {
        if (isMain)/// 주무기 추가
        {
            if (GameManager.Instance.GetJewel() >= selectData.unlockCost)
            {
                GameManager.Instance.UnlockWeapon(selectData);
                selectWeapon = GameManager.Instance.ownedWeapons.Find(w => w.data.weaponName == selectData.weaponName); //버그픽스용 추가코드
                selectData = selectWeapon.data;   //버그 픽스용 추가코드
            }
        }
        else ///보조 무기 추가
        {
            if (GameManager.Instance.GetJewel() >= selectSubData.unlockCost)
            {
                GameManager.Instance.UnlockWeapon(selectSubData);
                selectSubWeapon = GameManager.Instance.ownedSubWeapons.Find(w => w.data.weaponName == selectSubData.weaponName); //버그픽스용 추가코드
                selectSubData = selectSubWeapon.data;   //버그 픽스용 추가코드
            }
        }
        UnlockEffect();
        unlockButton.gameObject.SetActive(false);
        reinforceButton.gameObject.SetActive(true);
        UpdateSelectUI();
        UIUpdate();
    }
    public void UIUpdate()
    {
        for (int i = 0; i < weaponUIs.Length; i++)
        {
            Button uiButton = weaponUIs[i].GetComponent<Button>();
            uiButton.interactable = true;
            Outline uiOutline = uiButton.GetComponent<Outline>();
            uiOutline.enabled = false;
            uiButton.gameObject.SetActive(false); // ui초기화 코드 추가 0804
        }
        reinforceButton.gameObject.SetActive(false);
        reinforceButton.interactable = true;
        unlockButton.gameObject.SetActive(false); //추가
        unlockButton.interactable = true;
        useGoldText.gameObject.SetActive(false);
        useJewelText.gameObject.SetActive(false);
        weaponHp.gameObject.SetActive(false);
        goldImage.gameObject.SetActive(false);
        jewelImage.gameObject.SetActive(false);
        
        if (isMain)
        {
            for (int i = 0; i < GameManager.Instance.allMainWeapons.Count; i++)
            {
                Button uiButton = weaponUIs[i].GetComponent<Button>();
                uiButton.gameObject.SetActive(true);
                uiButton.interactable = true;
                Outline uiOutline = uiButton.GetComponent<Outline>();
                if (weaponUIs[i].mainData == selectData)
                {
                    uiButton.interactable = false;
                    uiOutline.enabled = true;
                }
            }
            weaponDesc.text = selectData.weaponDesc;
            weaponHp.gameObject.SetActive(true);
            weaponHp.text = ("체력 : ") + selectData.maxLives.ToString();
            hasGoldText.text = GameManager.Instance.GetGold().ToString();
            hasJewelText.text = GameManager.Instance.GetJewel().ToString();
            useJewelText.text = selectData.unlockCost.ToString();
            weaponImage.sprite = selectData.weaponImage;
            reinforceText.color = enabledColor;
            if (selectWeapon != null && selectWeapon.data != null)
            {
                goldImage.gameObject.SetActive(true);
                weaponAtk.text = ("공격력 : ") + (int)(selectWeapon.levelDamage);
                currentWeaponAtk.text = ((int)selectWeapon.levelDamage).ToString();
                if(selectWeapon.level <= 0) weaponName.text = selectData.weaponName;
                else weaponName.text = selectData.weaponName + "+" + (selectWeapon.level);
                //hasGoldText.gameObject.SetActive(true);
                useGoldText.gameObject.SetActive(true);
                reinforceButton.gameObject.SetActive(true);
                goldImage.gameObject.SetActive(true);
                //useGoldImage.gameObject.SetActive(true);
                if (selectWeapon.level < 10)
                {
                    useGoldText.text =  selectData.upgradeCost[selectWeapon.level].ToString();
                    upgradeWeaponAtk.text = ((int)(selectWeapon.levelDamage + selectData.damagePerLevel)).ToString();
                }
                else
                {
                    useGoldText.text = "최대 레벨";
                    upgradeWeaponAtk.text = "최대 레벨";
                    goldImage.gameObject.SetActive(false);
                    reinforceButton.interactable = false;
                    reinforceText.color = disableColor;
                }

                //if (GameManager.Instance.GetGold() < selectData.upgradeCost[selectWeapon.level >= 10 ? 9 : selectWeapon.level] || selectWeapon.level >=10)
                if(GameManager.Instance.GetGold() < selectData.upgradeCost[selectWeapon.level >= 10 ? 9 : selectWeapon.level] || selectWeapon.level >= 10)//10까지 강화가안되요
                {
                    //reinforceButton.gameObject.SetActive(false);
                    reinforceButton.interactable = false;
                    reinforceText.color = disableColor;
                }
            }
            else
            {
                unlockButton.gameObject.SetActive(true);
                unlockText.color = enabledColor;
                unlockButton.interactable = true;
                weaponAtk.text = "공격력 : " + selectData.baseDamage.ToString();
                currentWeaponAtk.text = "미해금";
                upgradeWeaponAtk.text = selectData.baseDamage.ToString();
                weaponName.text = selectData.weaponName;
                useJewelText.gameObject.SetActive(true);
                unlockText.color = enabledColor;
                jewelImage.gameObject.SetActive(true);
                
                if (GameManager.Instance.GetJewel() < selectData.unlockCost)
                {
                    unlockButton.interactable = false;
                    unlockText.color = disableColor;
                }
            }
        }
            
        else
        {
            for (int i = 0; i < GameManager.Instance.allSubWeapons.Count; i++)
            {
                Button uiButton = weaponUIs[i].GetComponent<Button>();
                uiButton.gameObject.SetActive(true);
                uiButton.interactable = true;
                Outline uiOutline = uiButton.GetComponent<Outline>();
                if (weaponUIs[i].subData == selectSubData)
                {
                    uiButton.interactable = false;
                    uiOutline.enabled = true;
                }
            }
            weaponDesc.text = selectSubData.weaponDesc;
            //hasGoldText.text = GameManager.Instance.GetGold().ToString();
            useGoldText.text = selectSubData.upgradeCost.ToString();
            weaponImage.sprite = selectSubData.weaponSprite;
            //hasJewelText.text = GameManager.Instance.GetJewel().ToString();
            useJewelText.text = selectSubData.unlockCost.ToString();
            reinforceButton.gameObject.SetActive(true);
            if (selectSubWeapon != null && selectSubWeapon.data !=null)
            {
                if(selectSubWeapon.level <= 0) weaponName.text = selectSubData.weaponName;
                else weaponName.text = selectSubData.weaponName + "+" + (selectSubWeapon.level);
                
                weaponAtk.text = ("공격력 : ") + (int)selectSubWeapon.levelDamage;
                currentWeaponAtk.text = ((int)(selectSubWeapon.levelDamage)).ToString();
                //hasGoldText.gameObject.SetActive(true);
                useGoldText.gameObject.SetActive(true);
                goldImage.gameObject.SetActive(true);
                //useGoldImage.gameObject.SetActive(true);
                if (selectSubWeapon.level < 10)
                {
                    useGoldText.text = selectSubData.upgradeCost[selectSubWeapon.level].ToString();
                    upgradeWeaponAtk.text = ((int)(selectSubWeapon.levelDamage + selectSubData.damagePerLevel)).ToString();
                    reinforceText.color = enabledColor;
                }
                else
                {
                    useGoldText.text = "최대 레벨";
                    upgradeWeaponAtk.text = "최대 레벨";
                    goldImage.gameObject.SetActive(false);
                    reinforceButton.interactable = false;
                    reinforceText.color = disableColor;
                }

                if (GameManager.Instance.GetGold() < selectSubData.upgradeCost[selectSubWeapon.level >= 10 ? 9 : selectSubWeapon.level] || selectSubWeapon.level >=10)
                {
                    //reinforceButton.gameObject.SetActive(false);
                    reinforceButton.interactable = false;
                    reinforceText.color = disableColor;
                }
            }
            else
            {
                weaponAtk.text = ("공격력 : ") + selectSubData.baseDamage.ToString();
                unlockText.color = enabledColor;
                unlockButton.gameObject.SetActive(true);
                unlockButton.interactable = true;
                //if(selectSubData.weaponName != selectSubWeapon)
                currentWeaponAtk.text = "미해금";
                upgradeWeaponAtk.text = selectSubData.baseDamage.ToString();
                weaponName.text = selectSubData.weaponName;
                //hasJewelText.gameObject.SetActive(true);
                useJewelText.gameObject.SetActive(true);
                unlockButton.gameObject.SetActive(true);
                jewelImage.gameObject.SetActive(true);
                //useJewelImage.gameObject.SetActive(true);
                weaponAtk.text = ("공격력 : ") + selectSubData.baseDamage.ToString();
                if (GameManager.Instance.GetJewel() < selectSubData.unlockCost)
                {
                    unlockButton.interactable = false;
                    unlockText.color = disableColor;
                }
            }
        }
    }
    
    public void ClickExitButton()
    {
        gameObject.SetActive(false);
    }

    public void UpdateSelectUI()
    {
        if (isMain)
        {
            for (int i = 0; i < GameManager.Instance.allMainWeapons.Count; i++)
            {
                weaponUIs[i].UpdateUI(GameManager.Instance.allMainWeapons[i]);
            }
            for (int i = GameManager.Instance.allMainWeapons.Count; i < weaponUIs.Length; i++)
            {
                weaponUIs[i].UpdateUI();
            } 
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.allSubWeapons.Count; i++)
            {
                weaponUIs[i].UpdateUI(GameManager.Instance.allSubWeapons[i]);
            }

            for (int i = GameManager.Instance.allSubWeapons.Count; i < weaponUIs.Length; i++)
            {
                weaponUIs[i].UpdateUI();
            } 
        }
        UIUpdate();
    }
    public void ClickWeaponChangeButton()        //무기 변경
    {
        isMain = !isMain;
        if (isMain)
        {
            mainWeaponButton.interactable = false;
            subWeaponButton.interactable = true;
        }
        else
        {
            subWeaponButton.interactable = false;
            mainWeaponButton.interactable = true;
        }
        
        UIUpdate();
        UpdateSelectUI();
    }

    public void ReinforceEffect()
    {
        for (int i = 0; i < upgradeDirection.Length; i++)
        {
            if (!upgradeDirection[i].activeInHierarchy)
            {
                upgradeDirection[i].gameObject.SetActive(true);
                StartCoroutine(EffectTime(effectDurationTime, upgradeDirection[i]));
                return;
            }
        }
    }
    
    private void UnlockEffect()
    {
        unlockDirection.gameObject.SetActive(true);
        StartCoroutine(EffectTime(effectDurationTime, unlockDirection));
    }

    IEnumerator EffectTime(float durationTime, GameObject effect)
    {
        yield return new WaitForSeconds(durationTime);
        effect.SetActive(false);
    }

    public void ClickHintButton()
    {
        tutorialImageUI.gameObject.SetActive(true);
    }
}
