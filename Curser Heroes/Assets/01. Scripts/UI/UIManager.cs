
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
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

    public GameObject gameOver; // GameOverImage 오브젝트 추가

    public Button returnToTownButton; // 마을로 돌아가기 버튼 추가

    public Button restartButton;

    public BattleUI battleUI;
    
    public StageStartUI stageStartUI;
    
    public StageExitPanel stageExitPanel;
    
    public TutorialUI tutorialUI;

    public TextMeshProUGUI currentWaveText;
    public TextMeshProUGUI bestWaveText;
    public TextMeshProUGUI startGoldText;
    public TextMeshProUGUI earnedGoldText;
    public TextMeshProUGUI startJewelText;
    public TextMeshProUGUI earnedJewelText;
    private void Init()
    {
        battleUI.gameObject.SetActive(false);
        stageStartUI.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        gameOver.SetActive(false);
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

        // 버튼 클릭 이벤트 연결
        if (returnToTownButton != null)
        {
            returnToTownButton.onClick.AddListener(ReturnToTown);
        }
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
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

        if (GameManager.Instance != null)
        {
            GameManager.Instance.startSessionGold = GameManager.Instance.GetGold();
            GameManager.Instance.startSessionJewel = GameManager.Instance.GetJewel();
        }
    }
    
    public void StageEnd()
    {
        UpdateGameOverData();
        StartCoroutine(StageEndCoroutine());
    }
    private IEnumerator StageEndCoroutine()
    {
        float durationTime = 3.0f;
        float elapsedTime = 0.0f;

        // GameOverPanel(배경) 활성화 및 페이드 인 시작
        gameOverPanel.gameObject.SetActive(true);

        Cursor.visible = true;

        // 마우스 잠금 해제 (필요한 경우)
        Cursor.lockState = CursorLockMode.None;

        Color startColor = new Color(1, 1, 1, 0);
        Color endColor = new Color(1, 1, 1, 1);

        while (elapsedTime < durationTime)
        {
            float time = elapsedTime / durationTime;
            gameOverPanel.color = Color.Lerp(startColor, endColor, time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 페이드 인 완료 후 GameOverImage 활성화
        gameOver.SetActive(true);
    }
    private void UpdateGameOverData()
    {
        if (GameManager.Instance != null && WaveManager.Instance != null)
        {
            // GameManager와 WaveManager에서 필요한 데이터를 가져옵니다.
            int diedWave = WaveManager.Instance.CurrentWaveNumber; // WaveManager에서 직접 가져옴
            int displayedWave = diedWave > 1 ? diedWave - 1 : 0;
            int bestWave = GameManager.Instance.bestWave; // 또는 StageManager에서 가져옴
            int startGold = GameManager.Instance.startSessionGold;
            int earnedGold = GameManager.Instance.GetGold() - startGold; // 최종 골드 - 시작 골드
            int startJewel = GameManager.Instance.startSessionJewel;
            int earnedJewel = GameManager.Instance.GetJewel() - startJewel; // 최종 쥬얼 - 시작 쥬얼

            // Text 컴포넌트 업데이트
            currentWaveText.text = displayedWave.ToString();
            bestWaveText.text =  bestWave.ToString();
            startGoldText.text = startGold.ToString();
            earnedGoldText.text = earnedGold.ToString();
            startJewelText.text = startJewel.ToString();
            earnedJewelText.text = earnedJewel.ToString();
        }
        else
        {
            Debug.LogWarning("게임 데이터를 가져올 수 없습니다. GameManager 또는 WaveManager가 null입니다.");
        }
    }

    // 버튼 클릭 이벤트에 연결될 함수들
    private void ReturnToTown()
    {
        SceneManager.LoadScene("JW_StageSelectUI");
    }

    private void RestartGame()
    {
        // 현재 씬을 다시 로드하여 게임을 처음부터 시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
