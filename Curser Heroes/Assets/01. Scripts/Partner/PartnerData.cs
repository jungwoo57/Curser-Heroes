using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObjects/PartnerData", order = 4)]
public class PartnerData : ScriptableObject
{
    public string partnerName;       // 동료 이름
    public Sprite portraitSprite;    // UI에 쓸 그림
    public float gaugeMax = 100f;    // 게이지 최대치
    public string desc;
    public int unlockCost;
    public int[] upgradeCost;

    public BasePartner prefab;
}

