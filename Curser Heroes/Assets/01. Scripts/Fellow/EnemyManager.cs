using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private List<Monster> enemies = new List<Monster>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 몬스터가 등장하면 Enemy 스크립트가 호출
    public void RegisterEnemy(Monster enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void UnregisterEnemy(Monster enemy)
    {
        if (enemies.Contains(enemy))
            enemies.Remove(enemy);
    }

    public void StunAllEnemies(float duration)
    {
        foreach (var enemy in enemies) ;
           // enemy?.ApplyStun(duration);
    }

    public void DisableAttacksTemporarily(float duration)
    {
        foreach (var enemy in enemies) ;
            //enemy?.DisableAttack(duration);
    }

    public void DamageAllEnemies(int damage)
    {
        foreach (var enemy in enemies)
            enemy?.TakeDamage(damage);
    }

    public void ClearAll()
    {
        enemies.Clear();
    }
}
