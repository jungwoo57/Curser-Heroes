using UnityEngine;

public class DamageTest : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("몬스터 클릭됨. 제거합니다.");
        Destroy(gameObject); // 이 게임 오브젝트를 파괴
    }
}
