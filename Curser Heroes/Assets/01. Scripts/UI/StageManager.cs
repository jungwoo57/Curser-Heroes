using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private static StageManager instance;

    public static StageManager Instance
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
    
    [Header(("스테이지 정보"))] 
    [SerializeField] public List<StageData> stages;
    [SerializeField] public StageData selectStage;
    [SerializeField] public int[] bestWave; // 스테이지 선택 번호
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        selectStage = stages[0];
        //bestWave = new List<int>(new int[stages.Count]);
    }

    public void UpdateStage(int index)
    {
        selectStage = stages[index];
    }
    
}
