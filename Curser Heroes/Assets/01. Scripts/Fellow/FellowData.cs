using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewFellow", menuName = "Fellow/Create New Fellow")]
public class FellowData : ScriptableObject
{
    [Header("기본 정보")]
    public string fellowName;
    public Sprite fellowImage;
    public string description;

    [Header("특기 정보")]
    public FellowSpecialType specialType;
    public float specialDuration = 7f;

    [Header("프리팹")]
    public GameObject fellowPrefab;
}
