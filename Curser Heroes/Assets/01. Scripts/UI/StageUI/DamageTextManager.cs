using System.Collections;
using TMPro;
using UnityEngine;


public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager instance;

    [SerializeField] private GameObject damageText;
    [SerializeField] private Transform damageCanvas;
    [SerializeField] private float time;
    [SerializeField] private float offset;
        
    private void Awake()
    {
        instance = this;
    }

    public void ShowDamage(int damage, Vector3 monsterPosition)
    {
        Debug.Log("텍스트는나옴");
        for (int i = 0; i < damageCanvas.childCount; i++)
        {
            Transform child = damageCanvas.GetChild(i);
            if (!child.gameObject.activeInHierarchy)
            {
                child.position = monsterPosition; // 바로위에 뜨게 추가 ex) +offset
                child.gameObject.SetActive(true);

                TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
                if (text != null)
                {
                    text.text = damage.ToString();
                }
                StartCoroutine(DamageAnimation(child.gameObject));
                return;
            }
        }

        GameObject newText = Instantiate(damageText, damageCanvas);
        newText.transform.position = monsterPosition;
        newText.SetActive(true);
        
        TextMeshProUGUI dmgtext = newText.GetComponent<TextMeshProUGUI>();
        if (newText != null)
        {
            dmgtext.text = damage.ToString();
        }
        StartCoroutine(DamageAnimation(newText.gameObject));
    }

    IEnumerator CloseDamage(GameObject obj)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

    IEnumerator DamageAnimation(GameObject damageText)
    {
        int type = Random.Range(0, 2);
        Vector3 startPos = damageText.transform.position;
        Vector3 endPos = startPos + new Vector3(0.5f, 1.0f, 0);;
        if (type == 0)
        {
            endPos = startPos + new Vector3(-0.5f, 1.0f, 0);
        }
        float elapsed = 0;
        while (elapsed < time)
        {
            float t = elapsed / time;
            elapsed += Time.deltaTime;
            
            Vector3 pos = Vector3.Lerp(startPos, endPos, t);
            
            float posY = Mathf.Lerp(startPos.y, endPos.y, t) + 1.0f * Mathf.Sin(Mathf.PI  * t);
            
            damageText.transform.position = new Vector3(pos.x, posY, t);
            yield return null;
        }
        
        damageText.gameObject.SetActive(false);
    }

}
