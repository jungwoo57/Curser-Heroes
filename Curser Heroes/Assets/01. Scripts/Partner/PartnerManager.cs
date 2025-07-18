using UnityEngine;

public class PartnerManager : MonoBehaviour
{
    public static PartnerManager Instance { get; private set; }

    [Header("선택한 동료")]
    public PartnerData partnerData;
    public BasePartner selectPartner;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 제거
            return;
        }

        Instance = this;
    }
    
    private void Start()
    {
        if (!GameManager.Instance.equipPartner.data) return;
        partnerData = GameManager.Instance.equipPartner.data;
        BasePartner obj = Instantiate(partnerData.prefab, this.transform);
        selectPartner = obj.GetComponent<BasePartner>();
        selectPartner.level = GameManager.Instance.equipPartner.level;
        selectPartner.gameObject.SetActive(true);
    }
}
