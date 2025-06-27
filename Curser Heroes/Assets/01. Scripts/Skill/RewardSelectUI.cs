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

        healButton.onClick.AddListener(() => Select(0)); // 목숨
        goldButton.onClick.AddListener(() => Select(1));
        jewelButton.onClick.AddListener(() => Select(2));
    }

    private void Select(int choice)
    {
        onSelect?.Invoke(choice);
        Destroy(gameObject); // UI 제거
    }
}
