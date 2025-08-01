using System;
using TMPro;
using UnityEngine;

public class InHandCashUI : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI goldText;
   [SerializeField] private TextMeshProUGUI jewelText;

   private void Start()
   {
      goldText.text = GameManager.Instance.GetGold().ToString();
      jewelText.text = GameManager.Instance.GetJewel().ToString();

      GameManager.Instance.OnGoldChanged += UpdateUI;
      GameManager.Instance.OnJewelChanged += UpdateUI;
   }

   public void UpdateUI()
   {
      goldText.text = GameManager.Instance.GetGold().ToString();
      jewelText.text = GameManager.Instance.GetJewel().ToString();
   }
}
