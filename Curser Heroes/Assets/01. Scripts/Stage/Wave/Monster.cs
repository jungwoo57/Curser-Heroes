using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public System.Action<GameObject> onDeath;

    private int currentHp;
    private MonsterData monsterData;

    public void Setup(MonsterData data)
    {
        monsterData = data;
        currentHp = data.maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"{gameObject.name} 피해: {damage} / 남은 HP: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} 사망!");
        onDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }
    private void OnMouseDown()
    {
        Debug.Log("몬스터 클릭됨 -> 데미지 처리");
        TakeDamage(999);
    }
}
