using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : BaseMonster
{
    protected override void Attack()
    {
      

        Collider2D player = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask("Player"));
        if (player != null)
        {
          // player.GetComponent<Player>().TakeWeaponLifeDamage(damage);
        }
    }
    private void OnDrawGizmosSelected()
    { // 디버그용 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
