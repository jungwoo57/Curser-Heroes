
using UnityEngine;

public class StunImageFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    public void Init(Transform monster)
    {
        target = monster;
    }
    
    void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            return;
        }
    }
    
}
