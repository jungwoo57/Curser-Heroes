
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{

    private static UIManager instance;
    public bool isStart;
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

    public SpriteRenderer stageBackGround;
    
    public Image gameOverPanel;
    
    public BattleUI battleUI;
    
    public StageStartUI stageStartUI;
    
    public StageExitPanel stageExitPanel;
    
    public TutorialUI tutorialUI;
    private void Init()
    {
        battleUI.gameObject.SetActive(false);
        stageStartUI.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (StageManager.Instance != null)
        {
            stageBackGround.sprite = StageManager.Instance.selectStage.battleStageBackGround;
        }
        isStart = false;
        tutorialUI.gameObject.SetActive(false);
        if (!GameManager.Instance.useStage)
        {
            tutorialUI.gameObject.SetActive(true);
            tutorialUI.tutorialEnd = () =>
            {
                StageStart();
            };
        }
        else
        {
            StageStart();
        }

        AudioManager.Instance.PlayBgm(bgmType.battle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (!stageExitPanel.gameObject.activeSelf && isStart)
            {
                stageExitPanel.gameObject.SetActive(true);
            }
        }
    }

    [ContextMenu("스테이지시작")]
    public void StageStart()
    {
        battleUI.gameObject.SetActive(true);
        stageStartUI.gameObject.SetActive(true);
        stageStartUI.StartAnimation();
    }
    
    public void StageEnd()
    {
        StartCoroutine(StageEndCoroutine());
    }
    private IEnumerator StageEndCoroutine()
    {
        float durationTime = 3.0f;
        float elapsedTime = 0.0f;
        gameOverPanel.gameObject.SetActive(true);

        Color startColor = new Color(1,1,1,0);
        Color endColor = new Color(1,1,1,1);

        while (elapsedTime < durationTime)
        {
            float time = elapsedTime / durationTime;
            gameOverPanel.color = Color.Lerp(startColor, endColor, time);
            elapsedTime+= Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene("JW_StageSelectUI");
    }
    
}
