using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager instance;

    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private Transform damageCanvas;

    private void Awake()
    {
        instance = this;
    }

    public void ShowDamage(int damage, Vector3 position)
    {
        for (int i = 0; i < damageCanvas.childCount; i++)
        {
            Transform child = damageCanvas.GetChild(i);
            if (!child.gameObject.activeInHierarchy)
            {
                child.position = Camera.main.WorldToScreenPoint(position); // 바로위에 뜨게 추가
                child.gameObject.SetActive(true);
            }
        }
    }
}
