using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class PoolData
{
    public string key;
    public GameObject prefab;
    public int count = 10;
}
public class PoolingRefatory : MonoBehaviour
{
    public List<PoolData> poolDataList;
    public Transform monsterPool;
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    
    private static PoolingRefatory instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        GameObject MonsterPool = new GameObject("MonsterPool");
        MonsterPool.transform.parent = this.transform;
        monsterPool = MonsterPool.transform;
        InitPool();
    }

    public static PoolingRefatory Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    void InitPool()
    {
        foreach (PoolData poolData in poolDataList)
        {
            Queue<GameObject> pool = new Queue<GameObject>();

            for (int i = 0; i < poolData.count; i++)
            {
                GameObject obj = Instantiate(poolData.prefab, transform);
                Debug.Log("큐 생성 완료");
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            
            poolDictionary.Add(poolData.key, pool);
        }
    }

    public GameObject Get(string key)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.Log("키값없음");
            return null;
        }

        GameObject obj = null;
        if (poolDictionary[key].Count > 0)
        {
            obj = poolDictionary[key].Dequeue();
        }
        else
        {
            PoolData poolData = poolDataList.Find(p => p.key==key);
            obj = Instantiate(poolData.prefab, transform);
        }
        
        obj.SetActive(true);
        return obj;
    }
    public void Return(GameObject obj,string key)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.Log("키값 없음");
            return;
        }
        obj.SetActive(false);
        poolDictionary[key].Enqueue(obj);
        
    }

    [ContextMenu("리턴시키기")]
    public void TestReturn()
    {
        foreach (var pair in poolDictionary)
        {
            string key = pair.Key;
            Queue<GameObject> queue = pair.Value;

            // 현재 풀에서 나가 사용 중인 오브젝트들을 추적하려면, 씬 전체 탐색 필요
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            int returnCount = 0;

            foreach (var obj in allObjects)
            {
                // 아직 비활성화되지 않은 오브젝트만 찾는다
                if (obj.activeInHierarchy && poolDataList.Find(p => p.key == key)?.prefab.name == obj.name.Replace("(Clone)", "").Trim())
                {
                    // 큐에 이미 들어있는 건 제외 (사용 중인 것만 처리)
                    if (!queue.Contains(obj))
                    {
                        obj.SetActive(false);
                        queue.Enqueue(obj);
                        returnCount++;
                    }
                }
            }

            Debug.Log($"{key} 활성화된 오브젝트 {returnCount}개 리턴함 (총 큐: {queue.Count})");
        }
    }
}
