
using System;
using System.Collections;
using UnityEngine;

public class LadyEffect : MonoBehaviour
{
    [SerializeField]private float stunTime = 7.0f;

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            BaseMonster monster = other.gameObject.GetComponent<BaseMonster>();
            monster.Stun(stunTime);
            //monster.Stun(); //7초를 가져가야해서 이건 좀 봐야함
        }
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
}
