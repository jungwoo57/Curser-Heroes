using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StageSelectUI : MonoBehaviour
{
        [Header("UI 구성요소")]
        [SerializeField] private StageSelectUI stageSelectUI;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button stageChangeButtonleft;
        [SerializeField] private Button stageChangeButtonright;
        [SerializeField] private Image backGroundImage;
        [SerializeField] private Button stageStartButton;
        
        [Header("스테이지 정보")] 
        [SerializeField] private TextMeshProUGUI bestScoreText;
        [SerializeField] private TextMeshProUGUI bestWaveText;
        [SerializeField] private TextMeshProUGUI stageNameText;
        [SerializeField] private TextMeshProUGUI stageWarningText;
        [SerializeField] private Image mainWeaponImage;
        [SerializeField] private Image subWeaponImage;
        [SerializeField] private Image partnerImage;
        [SerializeField] private Button skillListButton;
        [SerializeField] private int stageIndex;        
        [SerializeField] private int maxStageIndex;
        [SerializeField] private int unlockStageWave;
        
        [Header("창 모음")] 
        [SerializeField] private GameObject weaponSelectUI;
        [SerializeField] private GameObject skillListPanel;
        
       

        void Awake()
        {
                Init();
        }
        
        public void Init()
        {
                stageIndex = 0; // 해당 부분 스테이지 구현 후 변경
                if (StageManager.Instance != null)
                {
                        maxStageIndex = StageManager.Instance.stages.Count - 1;
                }
                UpdateStageInfoUI();
        }

        private void OnEnable()
        {
                //maxStageIndex = GameManager.Instance.bestScore;
                UpdateStageInfoUI();
        }

        void Update()
        {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                        ClickHomeButton();
                }
        }
        public void ClickNextStageButton(int value)
        {
                stageIndex += value;
                if (stageIndex < 0)
                {
                        stageIndex = maxStageIndex;
                        //return;
                }

                if (stageIndex > maxStageIndex)
                {
                        stageIndex = 0; // 첫 스테이지 로 변경
                        //return;
                }    // 해당 함수 부분 기획 의견 듣고 결정 (마지막 스테이지에서 처음 스테이지로 이동할지 아니면 넘어가 지지 않을지)
                
                Debug.Log(stageIndex);
                if (StageManager.Instance != null)
                {
                        StageManager.Instance.UpdateStage(stageIndex);
                }
                UpdateStageInfoUI();
                
        }

        public void UpdateStageInfoUI() // 해당 인덱스의 스테이지 정보 UPDATE
        { 
                stageWarningText.gameObject.SetActive(false); 
                stageStartButton.interactable = true;
              //bestScoreText.text = StageManager.Instance.selectStage.stageName;     
              //bestWaveText.text = GameManager.Instance.bestScore.ToString() + " WAVE"; // 가장 높은 웨이브 설정
              bestWaveText.text = StageManager.Instance.bestWave[stageIndex].ToString();
              stageNameText.text = StageManager.Instance.selectStage.stageName;
              backGroundImage.sprite = StageManager.Instance.selectStage.stageImage;
              if (stageIndex > 0 && StageManager.Instance.bestWave[stageIndex-1] < unlockStageWave)
              {
                      stageWarningText.gameObject.SetActive(true);
                      stageStartButton.interactable = false;
              }
        }

        public void UpdatePartnerInfoUI()
        {
                /*mainWeaponImage = null; 해당 부분 파트너 무기 서브무기 선택 완료 시 적용
                subWeaponImage = ;
                partnerImage = ;*/
        }

        public void ClickWeaponButton()
        {
                weaponSelectUI.SetActive(true);
        }
        
        public void ClickStageSelectButton()
        {
                stageSelectUI.gameObject.SetActive(true);   
        }
        
        public void ClickHomeButton()
        {
                stageSelectUI.gameObject.SetActive(false);           // 판넬 끄기 마을 보여주기
        }

        public void ClickEnterButton()
        {
         
                if (GameManager.Instance.selectSkills.Count != 12)
                {
                        Debug.Log("모든 스킬이 선택되지 않았습니다.");
                        return;
                }
                StartCoroutine(LoadSceneAndStartUI());
        }

        public void ClickSkillListButton()
        {
                Debug.Log("스킬창 출력");                   //테스트용 코드 
                skillListPanel.SetActive(true);
        }
        
        IEnumerator LoadSceneAndStartUI()
        {
                AsyncOperation async = SceneManager.LoadSceneAsync("BattleTest");
                //AsyncOperation async = SceneManager.LoadSceneAsync("JW_EquipPartner");
                while (!async.isDone) yield return null;
                
                yield return new WaitUntil(() => UIManager.Instance != null);
        }
        
        

}
