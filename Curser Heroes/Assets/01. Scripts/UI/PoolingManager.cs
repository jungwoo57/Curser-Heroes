using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    private static PoolingManager instance;

    public GameObject[] monsterPrefabs;

    List<GameObject>[] pools;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        pools = new List<GameObject>[monsterPrefabs.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public static PoolingManager Instance
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

    public GameObject GetMonster(int index)
    {
        GameObject obj = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                obj = item;
                obj.SetActive(true);
                break;
            }
        }

        if (!obj)
        {
            obj = Instantiate(monsterPrefabs[index], transform);
            pools[index].Add(obj);
        }
        return obj;
    }

    public void ReturnMonster(GameObject monster)
    {
        monster.SetActive(false);
    }
    

}
