using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIPanel : MonoBehaviour
{
    [SerializeField] public SkillUI[] skills;



    public void SkillUpdate()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            //skills[i].UpdateSkillUI();
        }
    }
}
