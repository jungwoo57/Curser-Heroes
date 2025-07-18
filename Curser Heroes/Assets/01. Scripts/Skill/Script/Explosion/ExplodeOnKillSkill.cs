using UnityEngine;

public class ExplodeOnKillSkill : MonoBehaviour
{
    public static GameObject explosionPrefab;
    public GameObject bombPrefab; // 폭탄 프리팹을 여기 연결

    public void TriggerExplosion(Vector3 position, int damage, float radius, LayerMask monsterLayer)
    {
        if (bombPrefab != null)
        {
            GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
            var comp = bomb.GetComponent<ExplodingBomb>();
            if (comp != null)
                comp.Init(damage, radius, monsterLayer);
            else
                Debug.LogWarning("폭탄 프리팹에 ExplodingBomb 컴포넌트가 없습니다.");
        }
        else
        {
            Debug.LogWarning("폭탄 프리팹(bombPrefab)이 null입니다!");
        }
    }
}
