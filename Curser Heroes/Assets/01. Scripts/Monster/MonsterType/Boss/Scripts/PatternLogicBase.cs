using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternLogicBase : MonoBehaviour
{
    public abstract IEnumerator Execute(BossPatternController controller);

}


