using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager instance;

    [SerializeField] private GameObject damageText;
    [SerializeField] private Transform damageCanvas;
    [SerializeField] private float time;

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
                child.position = Camera.main.WorldToScreenPoint(position); // 바로위에 뜨게 추가 ex) +offset
                child.gameObject.SetActive(true);

                TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
                if (text != null)
                {
                    text.text = damage.ToString();
                }
                StartCoroutine(CloseDamage(child.gameObject));
                return;
            }
        }

        GameObject newText = Instantiate(damageText, damageCanvas);
        newText.transform.position = Camera.main.WorldToScreenPoint(position);
        newText.SetActive(true);
        
        TextMeshProUGUI dmgtext = newText.GetComponent<TextMeshProUGUI>();
        if (newText != null)
        {
            dmgtext.text = damage.ToString();
        }
    }

    IEnumerator CloseDamage(GameObject obj)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

}
