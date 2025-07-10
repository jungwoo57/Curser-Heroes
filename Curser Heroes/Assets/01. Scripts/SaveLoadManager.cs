using System;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private string path;

    public static SaveLoadManager instance;

    private SaveData saveData;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        path = Application.persistentDataPath + "/save.data";
        saveData = new SaveData();
    }
    

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log(path + "저장완료" + json);    
    }

   

    public SaveData Load()
    {
        SaveData loadData = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
        Debug.Log("데이터 로드" + loadData);
        return loadData;
    }
}
