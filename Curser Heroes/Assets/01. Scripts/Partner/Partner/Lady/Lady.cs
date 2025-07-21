
using UnityEngine;

public class Lady : BasePartner
{
    public GameObject stunArea;
    protected override void ActivateSkill()
    {
        stunArea.SetActive(true);
    }
    
    
}
