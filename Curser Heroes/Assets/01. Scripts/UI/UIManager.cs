
using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private static UIManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        Init();
    }
    
    public static UIManager Instance
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

   

    public BattleUI battleUI;
    
    public StageStartUI stageStartUI;
    
    public WaveManager waveManager;  /// 추후 인스펙터에서 할당하지 않고 사용
                                    
    private void Init()
    {
        battleUI.gameObject.SetActive(false);
        stageStartUI.gameObject.SetActive(false);
    }

    private void Start()
    {
        StageStart();
    }
    
    [ContextMenu("스테이지시작")]
    public void StageStart()
    {
        battleUI.gameObject.SetActive(true);
        stageStartUI.gameObject.SetActive(true);
        stageStartUI.StartAnimation();
    }
    
    
}
