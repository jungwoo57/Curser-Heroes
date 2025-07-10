
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private string path;

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
    }
}
