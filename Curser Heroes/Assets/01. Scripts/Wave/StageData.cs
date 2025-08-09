using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/StageData")]
public class StageData : ScriptableObject
{
    public int stageNumber;

    [Header("몬스터 및 보스 풀")]
    public List<MonsterData> monsterPool;

    [Tooltip("스테이지당 등장하는 보스 (1마리만 설정)")]
    public BossData boss;

    [Header("스테이지 정보")] 
    public string stageName;
    public Sprite stageImage;    // 스테이지 선택창 백그라운드
    public Sprite battleStageBackGround;  // 전투 화면 백그라운드
    public AudioClip battleBgm;
}