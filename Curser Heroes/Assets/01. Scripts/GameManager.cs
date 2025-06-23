using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int gold = 0;
    private int jewel = 0;

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"골드 획득: {amount} / 총 골드: {gold}");
    }

    public void AddJewel(int amount)
    {
        jewel += amount;
        Debug.Log($"쥬얼 획득: {amount} / 총 쥬얼: {jewel}");
    }
}
