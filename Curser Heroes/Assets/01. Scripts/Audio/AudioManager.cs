using UnityEngine;

public enum HitType { Monster, Cursor } // 열거형 
public enum bgmType { main, battle, title, gameOver }

public enum buttonType { title, village, upgrade}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [Header("오디오 클립")]
    public AudioClip monsterHitClip, cursorHitClip;
    public AudioClip mainBgm, titleBgm, gameOverBgm;
    public AudioClip titlebuttonClip;
    public AudioClip uibuttonClip;
    public AudioClip upgradeClip;

    [Header("스테이지 데이터")]
    public StageData[] stageDatas;
    
    [Header("오디오 소스")]
    public AudioSource bgmSource;  // 추가된 bgm 소스
    public AudioSource src;    // 기존 효과음 ()
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();
        src = audioSources[0];
        bgmSource = audioSources[1];
        PlayBgm(bgmType.title);
    }

    public void PlayBgm(bgmType type, int stageNumber = 1)
    {
        bgmSource.Stop();
        
        switch (type)
        {
            case bgmType.main: 
                bgmSource.clip = mainBgm;
                break;
            case bgmType.title:
                bgmSource.clip = titleBgm;
                break;
            case bgmType.gameOver:
                bgmSource.clip = gameOverBgm; // ⭐️ 게임 오버 BGM 할당
                break;
            case bgmType.battle:
                //스테이지 번호에 맞는 StageData를 찾아 BGM을 재생합니다.
                AudioClip battleBgmToPlay = null;
                foreach (StageData data in stageDatas)
                {
                    if (data.stageNumber == stageNumber)
                    {
                        battleBgmToPlay = data.battleBgm;
                        break;
                    }
                }

                if (battleBgmToPlay != null)
                {
                    bgmSource.clip = battleBgmToPlay;
                }
                else
                {
                    Debug.LogWarning("StageData not found for stage number: " + stageNumber);
                }
                break;
        }
        bgmSource.loop = true;
        bgmSource.Play();
    }
    public void PlayHitSound(HitType type)
    {
        
        AudioClip clip;

        if (type == HitType.Monster)
        {
            clip = monsterHitClip;
        }
        else
        {
            clip = cursorHitClip;
        }

        src.PlayOneShot(clip);
    }

    public void PlayButtonSound(buttonType type)
    {
        AudioClip clip;

        switch (type)
        {
            case buttonType.title:
                clip = titlebuttonClip;
                src.PlayOneShot(clip);
                break;
            case buttonType.village:
                clip = uibuttonClip;
                src.PlayOneShot(clip);
                break;
            case buttonType.upgrade:
                clip = upgradeClip;
                src.PlayOneShot(clip);
                break;
        }
    }
}