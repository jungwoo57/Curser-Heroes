
using System;
using System.Collections;
using UnityEngine;

public class LadyEffect : MonoBehaviour
{
    private float stunTime = 7.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            BaseMonster monster = other.gameObject.GetComponent<BaseMonster>();
            StartCoroutine(stun(monster));
            //monster.Stun(); //7초를 가져가야해서 이건 좀 봐야함
        }
    }

    private IEnumerator stun(BaseMonster monster)
    {
        monster.Stun();
        float duration = 0;
        while (duration < stunTime)
        {
            duration += Time.deltaTime;
            yield return null;
        }
        monster.UnStun();
        
        gameObject.SetActive(false);
    }
}
