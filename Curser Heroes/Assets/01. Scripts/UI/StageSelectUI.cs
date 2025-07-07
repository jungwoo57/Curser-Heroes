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
        
        
        [Header("스테이지 정보")] 
        [SerializeField] private TextMeshProUGUI bestScoreText;
        [SerializeField] private TextMeshProUGUI bestWaveText;
        [SerializeField] private TextMeshProUGUI stageNameText;
        [SerializeField] private Image mainWeaponImage;
        [SerializeField] private Image subWeaponImage;
        [SerializeField] private Image partnerImage;
        [SerializeField] private Button skillListButton;

        [Header("창 모음")] 
        [SerializeField] private GameObject weaponSelectUI;
        [SerializeField] private GameObject skillListPanel;
        
        private int stageIndex;
        private int maxStageIndex;

        void Awake()
        {
                Init();
        }
        
        public void Init()
        {
                stageIndex = 1;
                maxStageIndex = 5; // 해당 부분 스테이지 구현 후 변경
        }

        public void ClickNextStageButton(int value)
        {
                stageIndex += value;
                if (stageIndex < 1)
                {
                        stageIndex = 1;
                        return;
                }

                if (stageIndex > maxStageIndex)
                {
                        stageIndex = maxStageIndex;
                        return;
                }    // 해당 함수 부분 기획 의견 듣고 결정 (마지막 스테이지에서 처음 스테이지로 이동할지 아니면 넘어가 지지 않을지)
                
                Debug.Log(stageIndex);
                UpdateStageInfoUI(stageIndex);
        }

        public void UpdateStageInfoUI(int index) // 해당 인덱스의 스테이지 정보 UPDATE
        {
              Debug.Log("UI업데이트 임시 출력");   // TEST용 코드
              //  bestScoreText.text = stage[index].maxScore.ToString()     예시 코드 스테이지 제작 후 결정
              bestWaveText.text = GameManager.Instance.bestScore.ToString(); // 가장 높은 웨이브 설정
              //stageNameText.text = stage[index].name;                     예시 코드 스테이지 제작 후 결정
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
                Debug.Log("게임 시작");                     // 테스트용 코드
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
                
                while (!async.isDone) yield return null;
                
                yield return new WaitUntil(() => UIManager.Instance != null);
        }
        
        

}
