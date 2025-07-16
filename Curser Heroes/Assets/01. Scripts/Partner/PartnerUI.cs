using UnityEngine;
using UnityEngine.UI;

public class PartnerUI : MonoBehaviour
{
  
    [SerializeField] private Image gaugeFillImage;   // 게이지 채워지는 부분

    public void Configure(Sprite portrait)
    {

        if (gaugeFillImage != null)
            gaugeFillImage.fillAmount = 0f;  // 게이지 초기화 
    }

    public void UpdateGauge(float normalized)
    {
        if (gaugeFillImage == null) return;
        gaugeFillImage.fillAmount = Mathf.Clamp01(normalized);
    }
}
