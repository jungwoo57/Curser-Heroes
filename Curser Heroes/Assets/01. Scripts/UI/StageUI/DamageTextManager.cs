using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager instance;

    [SerializeField] private GameObject damageText;
    [SerializeField] private Transform damageCanvas;
    [SerializeField] private Transform stunCanvas;
    [SerializeField] private float time;
    [SerializeField] private float offset;
    [SerializeField] private GameObject stunImage;
    [SerializeField] private Sprite stunSprite;
    [SerializeField] private float stunOffset;
    private void Awake()
    {
        instance = this;
    }

    public void ShowDamage(int damage, Vector3 monsterPosition)
    {
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

    public void ShowStun(Transform monsterPosition, float duration)
    {
        for (int i = 0; i < stunCanvas.childCount; i++)
        {
            Transform child = stunCanvas.GetChild(i);
            if (!child.gameObject.activeInHierarchy)
            {
                child.position = monsterPosition.position + Vector3.up * stunOffset;
                child.gameObject.SetActive(true);
                StunImageFollow follow = child.GetComponent<StunImageFollow>();
                follow.Init(monsterPosition);
                Image image = child.GetComponent<Image>();
                if (image != null)
                {
                    image.sprite = stunSprite;
                }
                StartCoroutine(CloseStun(child.gameObject, duration));
                return;
                // 스턴 애니메이션
            }
        }
        
        GameObject newStun = Instantiate(stunImage, stunCanvas);
        newStun.transform.position = monsterPosition.position + Vector3.up * stunOffset;
        
        StunImageFollow follows = newStun.GetComponent<StunImageFollow>();
        follows.Init(monsterPosition);
        
        Image newImage = newStun.GetComponent<Image>();
        if (newImage != null)
        {
            newImage.sprite = stunSprite;
        }
        StartCoroutine(CloseStun(newStun.gameObject, duration));
    }

    IEnumerator CloseStun(GameObject stunobj, float durationTime)
    {
        yield return new WaitForSeconds(durationTime);
        stunobj.SetActive(false);
    }
}
