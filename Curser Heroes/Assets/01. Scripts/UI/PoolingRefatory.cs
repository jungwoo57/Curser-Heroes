using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PoolingRefatory : MonoBehaviour
{
    public GameObject[] monsterPrefabs;
    public GameObject[] projectilePrefabs;
    
    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, Transform> poolParents = new Dictionary<string, Transform>();
    
    private Transform monsterPool;
    private Transform projectilePool;
    void Awake()
    {
        monsterPool = new GameObject("MonsterPool").transform;
        projectilePool = new GameObject("ProjectilePool").transform;
        
        foreach (GameObject obj in monsterPrefabs)
        {
            string key = obj.name;
            pool[key] = new Queue<GameObject>();
            poolParents[key] = monsterPool;
        }
        
        foreach (GameObject obj in projectilePrefabs)
        {
            string key = obj.name;
            pool[key] = new Queue<GameObject>();
            poolParents[key] = projectilePool;
        }
    }


    public GameObject GetMonster(string key)
    {
        if (!pool.ContainsKey(key)) return null;
        GameObject obj;
        
        Queue<GameObject> queue = pool[key];
        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            obj = queue.Dequeue();
        }
        else
        {
            //GameObject prefab = Sy
        }

        return null;
    }
}
