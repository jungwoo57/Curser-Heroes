using UnityEngine;

public class KingBlueparipari : Blueparipari
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject bluePrefab;
    [SerializeField] private int spawnCount = 5;

    protected override void Die()
    {
        SpawnMinions();
        base.Die();  
    }

    private void SpawnMinions()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 0.5f;
            Instantiate(bluePrefab, (Vector2)transform.position + offset, Quaternion.identity);
        }
    }
}
