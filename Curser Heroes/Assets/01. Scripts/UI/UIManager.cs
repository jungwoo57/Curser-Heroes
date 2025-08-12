
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

        if (StageManager.Instance != null && StageManager.Instance.selectStage != null)
        {
            int currentStageNumber = StageManager.Instance.selectStage.stageNumber;
            AudioManager.Instance.PlayBgm(bgmType.battle, currentStageNumber);
        }
        else
        {
            // 예외 처리: StageManager나 selectStage가 null일 경우
            Debug.LogWarning("StageManager or selectStage is not available. Cannot play BGM.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Exit Panel이 현재 활성화 상태인지 확인
            bool isPanelActive = stageExitPanel.gameObject.activeSelf;

            // isStart 상태이고, 패널이 비활성화 상태일 때 (처음 ESC를 누를 때)
            if (!isPanelActive && isStart)
            {
                stageExitPanel.gameObject.SetActive(true);
                // 게임 시간을 멈춤
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            // 패널이 활성화 상태일 때 (ESC를 다시 눌러 닫을 때)
            else if (isPanelActive)
            {
                stageExitPanel.gameObject.SetActive(false);

                // SkillSelectUI와 RewardSelectUI가 모두 활성화되지 않았을 때만 시간을 재개
                var skillSelectUI = GameObject.FindObjectOfType<SkillSelectUI>();
                var rewardSelectUI = GameObject.FindObjectOfType<RewardSelectUI>();

                if (skillSelectUI == null && rewardSelectUI == null)
                {
                    // 게임 시간을 재개
                    Time.timeScale = 1f;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Confined;
                }
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

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBgm(bgmType.gameOver);
        }
    }
    private IEnumerator StageEndCoroutine()
    {
        float durationTime = 3.0f;
        float elapsedTime = 0.0f;

        // GameOverPanel(배경) 활성화 및 페이드 인 시작
        gameOverPanel.gameObject.SetActive(true);

        // 게임 진행 멈추기
        Time.timeScale = 0f;

        // 마우스 커서 보이게 하기
        Cursor.visible = true;

        // 마우스 잠금 해제 (필요한 경우)
        Cursor.lockState = CursorLockMode.None;

        Color startColor = new Color(1, 1, 1, 0);
        Color endColor = new Color(1, 1, 1, 1);

        while (elapsedTime < durationTime)
        {
            float time = elapsedTime / durationTime;
            gameOverPanel.color = Color.Lerp(startColor, endColor, time);
            elapsedTime += Time.unscaledDeltaTime; // Time.unscaledDeltaTime을 사용하여 독립적으로 동작
            yield return null;
        }

        // 페이드 인 완료 후 GameOverImage 활성화
        gameOver.SetActive(true);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Save();
        }
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
        Time.timeScale = 1f;
        SceneManager.LoadScene("JW_StageSelectUI");
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        // 현재 씬을 다시 로드하여 게임을 처음부터 시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
