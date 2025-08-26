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
        path = Application.persistentDataPath + "/save.data";
        saveData = new SaveData();
    }

    void Start()
    {
        //if (GameManager.Instance != null)
        //{
        //    GameManager.Instance.Load();
        //}
    }
    

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public SaveData Load()
    {
        if(!File.Exists(path)) return null;
        SaveData loadData = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
        return loadData;
    }

    [ContextMenu("삭제")]
    public void Delete()
    {
        File.Delete(path);
    }
}
