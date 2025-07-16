using System;
using UnityEngine;
using UnityEngine.UI;

public class RewardSelectUI : MonoBehaviour
{
    public Button healButton;
    public Button goldButton;
    public Button jewelButton;

    private Action<int> onSelect;

    public void Init(Action<int> callback)
    {
        onSelect = callback;

        healButton.onClick.AddListener(() => Select(0)); // 0: 목숨 회복
        goldButton.onClick.AddListener(() => Select(1)); // 1: 골드
        jewelButton.onClick.AddListener(() => Select(2)); // 2: 쥬얼
    }

    private void Select(int choice)
    {
        onSelect?.Invoke(choice);
        Destroy(gameObject);
    }
}
